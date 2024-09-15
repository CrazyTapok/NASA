export const generateYearArray = (minYear: number, maxYear: number): number[] => {
  return Array.from({ length: maxYear - minYear + 1 }, (_, i) => minYear + i);
};
