export const formatDate = (value: Date | string): string =>
  (value instanceof Date ? value : new Date(value)).toLocaleString('nl-BE', {
    dateStyle: 'short',
    timeStyle: 'short',
  });
