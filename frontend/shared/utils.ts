/**
 * Checks if a string value is null, undefined, or whitespace.
 * @param value The value to check.
 * @returns True if the value is null, undefined, or empty; otherwise, false.
 */
export const stringIsNullOrWhitespace = (value: string | null | undefined): boolean =>
  !value || !value.trim();

export const pascalCastToCamelCase = (value: string): string =>
  value.charAt(0).toLowerCase() + value.slice(1);

export const mapProblemDetailsErrorsToExternalErrors = (
  errors: Record<string, string[]>,
): Record<string, string[]> => {
  const mappedErrors: Record<string, string[]> = {};
  for (const [key, messages] of Object.entries(errors)) {
    // Convert PascalCase (Name) to camelCase (name)
    const fieldName = pascalCastToCamelCase(key);
    mappedErrors[fieldName] = messages;
  }
  return mappedErrors;
};
