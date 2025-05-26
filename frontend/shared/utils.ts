/**
 * Checks if a string value is null, undefined, or whitespace.
 * @param value The value to check.
 * @returns True if the value is null, undefined, or empty; otherwise, false.
 */
export const stringIsNullOrWhitespace = (value: string | null | undefined): boolean =>
  !value || !value.trim();
