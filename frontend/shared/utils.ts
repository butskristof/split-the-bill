/**
 * Checks if a string value is null, undefined, or whitespace.
 * @param value The value to check.
 * @returns True if the value is null, undefined, or empty; otherwise, false.
 */
export const stringIsNullOrWhitespace = (value: string | null | undefined): boolean =>
  !value || !value.trim();

export const pascalCaseToCamelCase = (value: string): string =>
  value.charAt(0).toLowerCase() + value.slice(1);

export const mapProblemDetailsErrorsToExternalErrors = (
  errors: Record<string, string[]>,
): Record<string, string[]> => {
  const mappedErrors: Record<string, string[]> = {};
  for (const [key, messages] of Object.entries(errors)) {
    // Convert PascalCase (Name) to camelCase (name)
    const fieldName = pascalCaseToCamelCase(key);
    mappedErrors[fieldName] = messages;
  }
  return mappedErrors;
};

// Formatting utilities
const LOCALE = 'en-BE'; // European formatting with English labels

/**
 * Formats a date as DD/MM/YYYY (e.g., "13/10/2025")
 * @param date The date to format
 * @returns Formatted date string
 */
export const formatDate = (date: Date | string): string => {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return new Intl.DateTimeFormat(LOCALE, {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  }).format(dateObj);
};

/**
 * Formats a date and time as DD/MM/YYYY, HH:MM (e.g., "13/10/2025, 14:30")
 * @param date The date to format
 * @returns Formatted date-time string
 */
export const formatDateTime = (date: Date | string): string => {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return new Intl.DateTimeFormat(LOCALE, {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  }).format(dateObj);
};

/**
 * Formats a number as EUR currency with European formatting (e.g., "€ 12,50")
 * Uses comma as decimal separator and space after currency symbol
 * @param amount The amount to format
 * @returns Formatted currency string
 */
export const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat('nl-BE', {
    style: 'currency',
    currency: 'EUR',
  }).format(amount);
};
