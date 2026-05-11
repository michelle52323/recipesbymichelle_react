import { isIOS, isAndroid, isMobileTouchDevice } from './config';
export function renderNumberDisplayBySystem(
    input: string | null | undefined,
    measurementSystem: "Imperial" | "Metric" | null
): string {
    if (!input) return "";

    if (measurementSystem === "Imperial") {
        return renderMixedNumberHtml(input);
    }
    // Metric or null → return raw
    return input;
}

export function renderMixedNumberHtml(input: string): string {
    const isIos = isIOS();
    const isAndroidDevice = isAndroid();
    const isMobile = isMobileTouchDevice();

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
        if (isIos) {
            html += `
        <span class="fraction-ios">
          <span class="numerator">${numerator}</span>
          <span class="slash">⁄</span>
          <span class="denominator">${denominator}</span>
        </span>
      `;
        } else {
            const fractionClass = isMobile ? "fraction-mobile" : "fraction";
            html += `<span class="${fractionClass}">${spaceFraction(numerator.toString(), isAndroidDevice)}⁄${spaceFraction(denominator.toString(), isAndroidDevice)}</span>`;
        }
    }

    //return html.trim();
    return (isAndroidDevice) ? html.trim() + "&nbsp;" : html.trim()
    //return isAndroid() ? html.trim() + " " : html.trim();
}

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

export function renderStep(input: string): string {
    if (!input) return "";

    // 1. Convert "degree" → "°"
    const withDegrees = convertDegreeToSymbol(input);

    // 2. Render fractions / mixed numbers
    return renderAllMixedNumbersHtml(withDegrees);
}


export function convertDegreeToSymbol(input: string): string {
    if (!input) return "";

    // Replace "degree" or "degrees" with the symbol
    let output = input.replace(/\bdegrees?\b/gi, "°");

    // Remove a space directly before the symbol
    output = output.replace(/\s+°/g, "°");

    return output;
}




export function renderAllMixedNumbersHtml(input: string): string {
    if (!input) return "";

    const fractionRegex = /(\d+\s+\d+\/\d+|\d+\/\d+)/g;

    return input.replace(fractionRegex, (match) => {
        return renderMixedNumberHtml(match);
    });
}



