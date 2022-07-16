export function truncateToMaxChars(text: string, charCount: number): string {
    if (text.length < charCount) {
        return text;
    }
    let trimmedString = text.substr(0, charCount);
    if (trimmedString.lastIndexOf(' ') === -1) {
        return trimmedString + '...';
    }
    return trimmedString.substr(0, Math.min(trimmedString.length, trimmedString.lastIndexOf(' '))) + '...';
}