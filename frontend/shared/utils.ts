/**
 * Checks if a string value is null, undefined, or whitespace.
 * @param value The value to check.
 * @returns True if the value is null, undefined, or empty; otherwise, false.
 */
export const stringIsNullOrWhitespace = (value: string | null | undefined): boolean =>
  !value || !value.trim();

export const getUpperCaseFirstLetter = (
  value:
    | string
    | null
    | undefined
    | Ref<string | null | undefined>
    | ComputedRef<string | null | undefined>,
): string => {
  const str = unref(value);
  return str?.charAt(0).toUpperCase() ?? '';
};

export const formatTimestamp = (timestamp: string | number): string => {
  const date = new Date(timestamp);
  return date.toLocaleString('nl-BE', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
  });
};

export const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('nl-BE', {
    style: 'currency',
    currency: 'EUR',
  }).format(value);
};
