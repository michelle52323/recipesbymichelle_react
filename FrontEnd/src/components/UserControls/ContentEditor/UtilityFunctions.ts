export function isTipTapEmpty (html: string) {
    if (!html) return true;

    // Remove whitespace between tags
    const cleaned = html
        .replace(/<p>\s*<\/p>/gi, "")   // remove <p></p> with any whitespace
        .replace(/<[^>]+>/g, "")        // remove all HTML tags
        .trim();

    return cleaned.length === 0;
};