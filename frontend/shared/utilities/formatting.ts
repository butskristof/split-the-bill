export const formatCurrency = (value: number, currency: string = 'EUR'): string => {
  return new Intl.NumberFormat('nl-BE', { style: 'currency', currency }).format(value);
};
