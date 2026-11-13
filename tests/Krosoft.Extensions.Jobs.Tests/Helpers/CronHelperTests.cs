using Cronos;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Jobs.Helpers;
using Krosoft.Extensions.Testing;

namespace Krosoft.Extensions.Jobs.Tests.Helpers;

[TestClass]
public class CronHelperTests : BaseTest
{
    [TestMethod]
    [Description("Le décalage d'une clé fixe est stable (valeur épinglée pour détecter un changement d'algorithme).")]
    public void GetSecondOffset_Ok()
    {
        Check.That(CronHelper.GetSecondOffset("flux-client-42")).IsEqualTo(15);
        Check.That(CronHelper.GetSecondOffset("3f2504e0-4f89-11d3-9a0c-0305e82c3301")).IsEqualTo(49);
    }

    [TestMethod]
    [Description("Le décalage est identique entre deux appels pour la même clé.")]
    public void GetSecondOffset_Deterministe()
    {
        var key = Guid.NewGuid().ToString();

        Check.That(CronHelper.GetSecondOffset(key)).IsEqualTo(CronHelper.GetSecondOffset(key));
    }

    [TestMethod]
    [Description("Les décalages sont toujours dans [0, 59] et bien répartis sur plusieurs centaines de clés.")]
    public void GetSecondOffset_Repartition()
    {
        var offsets = Enumerable.Range(0, 600)
                                .Select(i => CronHelper.GetSecondOffset($"client-{i}"))
                                .ToList();

        Check.That(offsets.All(x => x is >= 0 and <= 59)).IsTrue();
        Check.That(offsets.Distinct().Count()).IsStrictlyGreaterThan(50);
        Check.That(offsets.GroupBy(x => x).Max(g => g.Count())).IsStrictlyLessThan(30);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [Description("Une clé null ou vide lève une KrosoftTechnicalException.")]
    public void GetSecondOffset_CleVide(string? key)
    {
        Check.ThatCode(() => CronHelper.GetSecondOffset(key!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'key' est vide ou non renseignée.");
    }

    [TestMethod]
    [DataRow("*/1 * * * *", "15 */1 * * * *")]
    [DataRow("*/30 * * * *", "15 */30 * * * *")]
    [DataRow("  0   12 * *   1-5 ", "15 0 12 * * 1-5")]
    [Description("Un cron à 5 champs est préfixé par la seconde de décalage, espaces multiples normalisés.")]
    public void SpreadWithinMinute_CinqChamps(string cron, string expected)
    {
        Check.That(CronHelper.SpreadWithinMinute(cron, "flux-client-42")).IsEqualTo(expected);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("0 */5 * * * *")]
    [DataRow("@daily")]
    [Description("Un cron null, vide, à 6 champs ou une macro est renvoyé tel quel.")]
    public void SpreadWithinMinute_Inchange(string? cron)
    {
        Check.That(CronHelper.SpreadWithinMinute(cron, "flux-client-42")).IsEqualTo(cron);
    }

    [TestMethod]
    [DataRow("*/1 * * * *")]
    [DataRow("0 0 * * *")]
    [DataRow("15,45 8-18 * * MON-FRI")]
    [Description("Le cron produit est parsable par Cronos au format avec secondes.")]
    public void SpreadWithinMinute_ParsableCronos(string cron)
    {
        var result = CronHelper.SpreadWithinMinute(cron, "flux-client-42");

        Check.ThatCode(() => CronExpression.Parse(result!, CronFormat.IncludeSeconds)).DoesNotThrow();
    }
}
