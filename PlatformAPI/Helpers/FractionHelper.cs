using PlatformAPI.Models.Recipe;
using Microsoft.EntityFrameworkCore;
//using PlatformAPI.Pages.Recipe;
using Microsoft.AspNetCore.Html;
using PlatformAPI.Data;
using PlatformAPI.Enums;
using PlatformAPI.Helpers;


namespace PlatformAPI.Helpers
{

    public class Fraction
    {
        public int Numerator {  get; set; }

        public int Denominator { get; set; }
    }

    public class MixedNumber
    {
        public int WholePart { get; set; }

        public Fraction Fraction { get; set; }
    }

    public class DecimalParts
    {
        public int WholePart { get; set; }

        public float DecimalPart { get; set; }
    }
    public class FractionHelperDB
    {

        private readonly DbContext _context;

        public FractionHelperDB(DbContext context)
        {
            _context = context;
        }

        public async Task<List<FractionDecimal>> LoadFractionTableAsync()
        {
            return await _context.Set<FractionDecimal>().ToListAsync();
        }

    } 
    
    public class FractionHelper
    {


        public static string? GetFractionFromDecimal(float decimalValue, List<FractionDecimal> fractionTable)
        {
            var match = fractionTable.FirstOrDefault(fd =>
                fd.Primary && fd.Decimal.HasValue && Math.Abs((decimal)fd.Decimal.Value - (decimal)decimalValue) < 0.0001m);

            return match?.Fraction;
        }

        public static float? GetDecimalFromFraction(string fraction, List<FractionDecimal> fractionTable)
        {
            var match = fractionTable.FirstOrDefault(fd =>
                string.Equals(fd.Fraction, fraction, StringComparison.OrdinalIgnoreCase));

            return match?.Decimal;
        }

        public static DecimalParts ConvertDecimalToDecimalParts(float decimalValue)
        {
            DecimalParts parts = new DecimalParts();

            parts.WholePart = (int)Math.Floor(decimalValue); // Extract whole number
            parts.DecimalPart = decimalValue - parts.WholePart; // Extract fractional part

            return parts;
        }

        public static float ConvertDecimalPartsToDecimal(DecimalParts parts)
        {
            return parts.WholePart + parts.DecimalPart;
        }

        public static MixedNumber ConvertStringToMixedNumber(string mixedNumber)
        {
            if (string.IsNullOrWhiteSpace(mixedNumber))
                throw new ArgumentException("Input cannot be null or empty.", nameof(mixedNumber));

            mixedNumber = mixedNumber.Trim();

            var result = new MixedNumber();

            // Case: "1 3/4"
            if (mixedNumber.Contains(" "))
            {
                var parts = mixedNumber.Split(' ');
                result.WholePart = int.Parse(parts[0]);

                var fractionParts = parts[1].Split('/');
                result.Fraction = new Fraction
                {
                    Numerator = int.Parse(fractionParts[0]),
                    Denominator = int.Parse(fractionParts[1])
                };
            }
            // Case: "7/8"
            else if (mixedNumber.Contains("/"))
            {
                result.WholePart = 0;

                var fractionParts = mixedNumber.Split('/');
                result.Fraction = new Fraction
                {
                    Numerator = int.Parse(fractionParts[0]),
                    Denominator = int.Parse(fractionParts[1])
                };
            }
            // Case: whole number only (e.g., "2")
            else
            {
                result.WholePart = int.Parse(mixedNumber);
                result.Fraction = new Fraction { Numerator = 0, Denominator = 1 };
            }

            return result;
        }

        public static string ConvertMixedNumberToString(MixedNumber mixedNumber)
        {
            if (mixedNumber == null || mixedNumber.Fraction == null)
                throw new ArgumentNullException(nameof(mixedNumber), "MixedNumber or its Fraction cannot be null.");

            int whole = mixedNumber.WholePart;
            int numerator = mixedNumber.Fraction.Numerator;
            int denominator = mixedNumber.Fraction.Denominator;

            if (numerator == 0 || denominator == 0)
                return whole.ToString(); // Just the whole number

            if (whole == 0)
                return $"{numerator}/{denominator}"; // Just the fraction

            return $"{whole} {numerator}/{denominator}"; // Mixed number
        }

        public static Fraction ConvertStringToFraction(string fraction)
        {

            if (string.IsNullOrWhiteSpace(fraction))
            {
                return new Fraction
                {
                    Numerator = 0,
                    Denominator = 1
                };
            }

            var parts = fraction.Trim().Split('/');

            if (parts.Length != 2)
                throw new FormatException("Fraction must be in the format 'numerator/denominator'.");

            if (!int.TryParse(parts[0], out int numerator) || !int.TryParse(parts[1], out int denominator))
                throw new FormatException("Numerator and denominator must be valid integers.");

            if (denominator == 0)
                throw new DivideByZeroException("Denominator cannot be zero.");

            return new Fraction
            {
                Numerator = numerator,
                Denominator = denominator
            };
        }

        public static string ConvertFractionToString(Fraction fraction)
        {
            if (fraction == null || fraction.Denominator == 0)
                return "Invalid";

            return $"{fraction.Numerator}/{fraction.Denominator}";
        }

        #region Higher Level Functions

        public static string ConvertDbValueToString(float decimalValue, List<FractionDecimal> fractionTable)
        {

            DecimalParts decimalParts = ConvertDecimalToDecimalParts(decimalValue);

            string f = GetFractionFromDecimal(decimalParts.DecimalPart, fractionTable);

            Fraction fraction = ConvertStringToFraction(f);

            MixedNumber mixedNumber = new MixedNumber
            {
                WholePart = decimalParts.WholePart,
                Fraction = fraction
            };

            return ConvertMixedNumberToString(mixedNumber);
        }

        public static string ConvertDbValueToStringBySystem(float decimalValue, List<FractionDecimal> fractionTable, MeasurementSystem? measurementSystem)
        {
            if (measurementSystem == MeasurementSystem.Imperial)
                return ConvertDbValueToString(decimalValue, fractionTable);
            else if (measurementSystem == MeasurementSystem.Metric)
                return decimalValue.ToString();

            return string.Empty;
        }

        public static float ConvertStringToDbValue(string quantity, List<FractionDecimal> fractionTable)
        {
            MixedNumber mixedNumber = ConvertStringToMixedNumber(quantity);
            string frac = ConvertFractionToString(mixedNumber.Fraction);
            float? dec = GetDecimalFromFraction(frac, fractionTable);
            DecimalParts decimalParts = new DecimalParts { WholePart = mixedNumber.WholePart, DecimalPart = dec ?? 0f };
            float newQty = ConvertDecimalPartsToDecimal(decimalParts);
            return newQty;

        }

        public static float ConvertStringToDbValueBySystem(string quantity, List<FractionDecimal> fractionTable, MeasurementSystem? measurementSystem)
        {

            if (String.IsNullOrEmpty(quantity))
                return 0f;
            if (measurementSystem == MeasurementSystem.Imperial)
                return ConvertStringToDbValue(quantity, fractionTable);
            else if (measurementSystem == MeasurementSystem.Metric)
                return float.Parse(quantity);

            return 0;
        }

        #endregion

        #region Display Functions

        public static string ConvertQuantityBySystemAsync(
            MeasurementSystem? measurementSystem,
            float? quantity,
            List<FractionDecimal>? fractionTable)
            {
                // 1. Null quantity → empty string
                if (quantity == null)
                    return string.Empty;

                // 2. Metric → return raw number
                if (measurementSystem == MeasurementSystem.Metric)
                    return quantity.Value.ToString();

                // 4. Convert decimal to mixed number string
                string mixed = ConvertDbValueToString(quantity.Value, fractionTable);

                // 5. Normalize via ConvertStringToMixedNumber
                var parsed = ConvertStringToMixedNumber(mixed);

                return ConvertMixedNumberToString(parsed);
            }

        #endregion
    }
}
