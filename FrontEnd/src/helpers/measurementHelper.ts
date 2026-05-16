import { isIOS, isAndroid, isMobileTouchDevice } from './config';
import { mapFractionFontToClass } from './displayHelper';
import type { MeasurementUnit } from 'src/types/Measurement/MeasurementType';

//Rendering functions
export function renderNumberDisplayBySystem(
    input: string | null | undefined,
    measurementSystem: "Imperial" | "Metric" | null,
    recipeFont: "SansSerif" | "Serif" | "Handwritten"
): string {
    if (!input) return "";

    if (measurementSystem === "Imperial") {
        return renderMixedNumberHtml(input, recipeFont);
    }
    // Metric or null → return raw
    return input;
}

export function renderMixedNumberHtml(input: string, recipeFont: "SansSerif" | "Serif" | "Handwritten"): string {
    const isIos = isIOS();
    const isAndroidDevice = isAndroid();
    const isMobile = isMobileTouchDevice();

    const fractionFontClass = mapFractionFontToClass(recipeFont);

    const deviceType =
        isIos ? "iOS" :
            isAndroidDevice ? "Android" :
                "Desktop";


    const { whole, numerator, denominator } = parseMixedNumber(input);

    let html = "";

    // Whole number
    if (whole > 0) {
        html += `${whole}`;
    }

    // Space between whole and fraction
    if (whole > 0 && numerator > 0) {
        html += " ";
    }

    // Fraction
    if (numerator > 0) {
        //     if (isIos) {
        //         html += `
        //     <span class="fraction-ios ${fractionFontClass}">
        //       <span class="numerator">${numerator}</span>
        //       <span class="slash">⁄</span>
        //       <span class="denominator">${denominator}</span>
        //     </span>
        //   `;
        //     } else {
        //         const fractionClass = isMobile ? "fraction-mobile" : "fraction";
        //         html += `<span class="${fractionClass} ${fractionFontClass}">${spaceFraction(numerator.toString(), isAndroidDevice)}⁄${spaceFraction(denominator.toString(), isAndroidDevice)}</span>`;
        //     }
        html += `
        <span class="fraction ${fractionFontClass}">
          <span class="numerator">${numerator}</span>
          <span class="slash">⁄</span>
          <span class="denominator">${denominator}</span>
        </span>
      `;
        html = html.trim();

        html += (recipeFont == "Handwritten") ? "&nbsp" : "";
    }

    return html;

    return html.trim();
    //return (isAndroidDevice) ? html.trim() + "&nbsp;" : html.trim()
    //return isAndroid() ? html.trim() + " " : html.trim();
}

//------------------------------------------------------
// Render fraction according to font and device type 
//------------------------------------------------------

export function spaceFraction(input: string, isAndroidDevice: boolean) {
    if (!isAndroidDevice) return input;
    if (!input || input.length <= 1) return input;

    // Insert a space between each character
    return input.split("").join(" ");
}


function parseMixedNumber(input: string) {
    const trimmed = input.trim();

    // CASE 1: Mixed number "1 1/4"
    if (trimmed.includes(" ")) {
        const [wholeStr, fracStr] = trimmed.split(" ");
        const [num, den] = fracStr.split("/").map(Number);
        return {
            whole: parseInt(wholeStr, 10),
            numerator: num,
            denominator: den
        };
    }

    // CASE 2: Simple fraction "1/4"
    if (trimmed.includes("/")) {
        const [num, den] = trimmed.split("/").map(Number);
        return {
            whole: 0,
            numerator: num,
            denominator: den
        };
    }

    // CASE 3: Whole number "2"
    return {
        whole: parseInt(trimmed, 10),
        numerator: 0,
        denominator: 1
    };
}

export function renderStep(input: string, recipeFont: "SansSerif" | "Serif" | "Handwritten"): string {
    if (!input) return "";

    // 1. Convert "degree" → "°"
    const withDegrees = convertDegreeToSymbol(input);

    // 2. Render fractions / mixed numbers
    return renderAllMixedNumbersHtml(withDegrees, recipeFont);
}


export function convertDegreeToSymbol(input: string): string {
    if (!input) return "";

    // Replace "degree" or "degrees" with the symbol
    let output = input.replace(/\bdegrees?\b/gi, "°");

    // Remove a space directly before the symbol
    output = output.replace(/\s+°/g, "°");

    return output;
}

export function renderAllMixedNumbersHtml(input: string, recipeFont: "SansSerif" | "Serif" | "Handwritten"): string {
    if (!input) return "";

    const fractionRegex = /(\d+\s+\d+\/\d+|\d+\/\d+)/g;

    return input.replace(fractionRegex, (match) => {
        return renderMixedNumberHtml(match, recipeFont);
    });
}

//Quantity validation functions

export function trimQuantity(raw: string): string {
    if (!raw) return "";

    // Step 1: trim leading/trailing whitespace
    let s = raw.trim();

    // Step 2: collapse multiple spaces into one
    s = s.replace(/\s+/g, " ");

    // Step 3: remove spaces around slash
    // "1 / 2" → "1/2"
    s = s.replace(/\s*\/\s*/g, "/");

    // Step 4: remove spaces around decimal point
    // "1 . 25" → "1.25"
    s = s.replace(/\s*\.\s*/g, ".");

    // Step 5: ensure exactly one space between whole and fraction
    // "1    1/2" → "1 1/2"
    s = s.replace(/(\d)\s+(\d+\/\d+)/g, "$1 $2");

    return s;
}

export function validateQuantityNumeric(
    measurementSystem: "Imperial" | "Metric" | null,
    rawValue: string
) {
    const cleaned = trimQuantity(rawValue);

    if (!cleaned) {
        return { isValid: false, cleaned, error: "Quantity is required." };
    }

    // Reject negatives early
    if (cleaned.startsWith("-")) {
        return { isValid: false, cleaned, error: "Negative values are not allowed." };
    }

    if (measurementSystem === "Imperial") {
        const whole = /^\d+$/;
        const fraction = /^\d+\/\d+$/;
        const mixed = /^\d+ \d+\/\d+$/;

        if (whole.test(cleaned) || fraction.test(cleaned) || mixed.test(cleaned)) {
            return { isValid: true, cleaned };
        }

        return {
            isValid: false,
            cleaned,
            error: "(U.S.) Imperial quantities must be whole numbers, fractions, or mixed numbers."
        };
    }

    if (measurementSystem === "Metric") {
        const decimal = /^\d*\.?\d+$/;

        if (decimal.test(cleaned)) {
            return { isValid: true, cleaned };
        }

        return {
            isValid: false,
            cleaned,
            error: "Metric quantities must be whole numbers or decimals."
        };
    }

    return {
        isValid: false,
        cleaned,
        error: "Measurement system is required."
    };
}

//Unit validation functions
export function validateUnitInput(
    measurementSystem: number | null,
    input: string,
    isPlural: boolean,
    unitLookup: MeasurementUnit[]
): { isValid: boolean; cleaned: string; error?: string } {

    // 1. Measurement system required
    if (measurementSystem === null) {
        return {
            isValid: false,
            cleaned: input,
            error: "Measurement system is required."
        };
    }

    // 2. Normalize input for matching
    //const cleaned = input.trim().replace(/\s+/g, "");
    const cleaned = input.trim();

    if (!cleaned) {
        // Empty unit is allowed
        return { isValid: true, cleaned: "" };
    }

    // 3. Exceptions (fl oz variants)
    const exception = checkUnitExceptions(cleaned);
    if (exception) {
        return { isValid: true, cleaned: exception };
    }

    // 4. Full match
    const full = fullMatch(cleaned, isPlural, unitLookup);
    if (full) {
        return { isValid: true, cleaned: full };
    }

    // 5. Three-letter heuristic
    const partial3 = firstThreeMatch(cleaned, isPlural, unitLookup);
    if (partial3) {
        return { isValid: true, cleaned: partial3 };
    }

    // 6. Two-letter heuristic
    const partial = firstTwoMatch(cleaned, isPlural, unitLookup);
    if (partial) {
        return { isValid: true, cleaned: partial };
    }

    // 7. No match
    return {
        isValid: false,
        cleaned: input, // return original for onBlur
        error: `Invalid unit: "${input}".`
    };
}

function checkUnitExceptions(input: string): string | null {
    const normalized = input.trim().toLowerCase().replace(/\./g, "").replace(/\s+/g, "");

    const exceptions: Record<string, string> = {
        "floz": "fl oz",
        "fl.oz.": "fl oz",
        "floz.": "fl oz",
        "fl.oz": "fl oz",
        "pinc": "pinch"
    };

    return exceptions[normalized] || null;
}

function fullMatch(
    input: string,
    isPlural: boolean,
    unitList: MeasurementUnit[]
): string | null {

    const normalized = input.trim().toLowerCase();

    for (const unit of unitList) {
        // Abbreviation
        if (unit.abbreviation?.toLowerCase() === normalized) {
            return unit.abbreviation;
        }

        // Description or plural
        if (
            unit.description?.toLowerCase() === normalized ||
            unit.plural?.toLowerCase() === normalized
        ) {
            return isPlural ? unit.plural! : unit.description!;
        }
    }

    return null;
}

function firstTwoMatch(
    input: string,
    isPlural: boolean,
    unitList: MeasurementUnit[]
): string | null {

    const normalized = input.trim().toLowerCase();
    const prefix = normalized.slice(0, 2);

    for (const unit of unitList) {
        const descPrefix = unit.description?.toLowerCase().slice(0, 2);
        const pluralPrefix = unit.plural?.toLowerCase().slice(0, 2);
        const abbrevPrefix = unit.abbreviation?.toLowerCase().slice(0, 2);

        if (prefix === descPrefix || prefix === pluralPrefix) {
            return isPlural ? unit.plural! : unit.description!;
        }

        if (prefix === abbrevPrefix) {
            return unit.abbreviation!;
        }
    }

    return null;
}

function firstThreeMatch(
    input: string,
    isPlural: boolean,
    unitList: MeasurementUnit[]
): string | null {

    const normalized = input.trim().toLowerCase();

    const prefix = normalized.slice(0, 3); // use 3 letters now

    for (const unit of unitList) {
        const descPrefix = unit.description?.toLowerCase().slice(0, 3);
        const pluralPrefix = unit.plural?.toLowerCase().slice(0, 3);
        const abbrevPrefix = unit.abbreviation?.toLowerCase().slice(0, 3);

        if (prefix === descPrefix || prefix === pluralPrefix) {
            return isPlural ? unit.plural! : unit.description!;
        }

        if (prefix === abbrevPrefix) {
            return unit.abbreviation!;
        }
    }

    return null;
}

// Unit lookup functions
export function getAbbreviation(unit: string, unitLookup: MeasurementUnit[]): string {
    if (!unit || !unitLookup || unitLookup.length === 0) return unit;

    const normalized = unit.trim().toLowerCase();

    // Try to find a matching unit by any of its names
    const match = unitLookup.find(u =>
        u.abbreviation?.toLowerCase() === normalized ||
        u.description?.toLowerCase() === normalized ||
        u.plural?.toLowerCase() === normalized
    );

    // If no match → return input
    if (!match) return unit;

    // If abbreviation exists → return it
    if (match.abbreviation) return match.abbreviation;

    // Otherwise → return input
    return unit;
}



// Unit requires pluralization

export function requiresPlural(qtyString, measurementSystem) {
    if (!qtyString) return false;

    if (measurementSystem === "Imperial") {
        return requiresPluralImperial(qtyString);
    }
    else if (measurementSystem === "Metric") {
        return requiresPluralMetric(qtyString);
    }

}

function requiresPluralImperial(qtyString) {
    const normalized = qtyString.trim();

    // Match "1 1/2" or "1/2"
    const fractionMatch = normalized.match(/^(\d+)?\s*(\d+)\/(\d+)$/);

    let value;

    if (fractionMatch) {
        const whole = parseInt(fractionMatch[1] || "0", 10);
        const numerator = parseInt(fractionMatch[2], 10);
        const denominator = parseInt(fractionMatch[3], 10);
        value = whole + numerator / denominator;
    } else {
        value = parseFloat(normalized);
    }

    if (isNaN(value)) return false;

    return value > 1;
}


function requiresPluralMetric(qtyString) {
    const normalized = qtyString.trim();

    // Try parsing as float
    const value = parseFloat(normalized);

    // If it's not a number or less than or equal to 1, return false
    if (isNaN(value) || value <= 1.0) return false;

    // If greater than 1, return true (plural)
    return true;
}


// Formatting functions

export function renderUnit(unit) {
    if (unit == "" || unit == null)
        return "";
    else
        return unit + " ";
}