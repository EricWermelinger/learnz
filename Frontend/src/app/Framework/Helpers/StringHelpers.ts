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

export function formatTime(minutes: number) {
    if (!minutes) {
        return 0 + '\'';
    }
    if (minutes >= 60) {
        return Math.floor(minutes / 60) + 'h ' + (minutes % 60) + '\'';
    }
    return minutes + '\'';
}