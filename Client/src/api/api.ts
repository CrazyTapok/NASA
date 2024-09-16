import { MeteoriteGroup } from './model/meteoriteGroup';

export const getUniqueRecClass = async (): Promise<string[]> => {
  try {
    const response = await fetch('https://localhost:44342/api/Meteorite/GetUniqueRecClass');
    if (!response.ok) {
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }
    return response.json();
  } catch (error) {
    console.error('Failed to fetch unique rec classes:', error);
    throw error;
  }
};

export const getUniqueYears = async (): Promise<number[]> => {
  try {
    const response = await fetch('https://localhost:44342/api/Meteorite/GetUniqueYears');
    if (!response.ok) {
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }
    return response.json();
  } catch (error) {
    console.error('Failed to fetch unique years:', error);
    throw error;
  }
};

export const getMeteorites = async (
  startYear: number | null,
  endYear: number | null,
  recClass: string | null,
  namePart: string | null,
  sortBy: string | null,
  ascending: boolean,
): Promise<MeteoriteGroup[]> => {
  let url = 'https://localhost:44342/api/Meteorite/GetMeteorites?';

  if (startYear !== null) url += `startYear=${startYear}&`;
  if (endYear !== null) url += `endYear=${endYear}&`;
  if (recClass !== null) url += `recClass=${recClass}&`;
  if (namePart !== null) url += `namePart=${namePart}&`;
  if (sortBy !== null) url += `sortBy=${sortBy}&`;
  url += `ascending=${ascending}`;

  try {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }
    return response.json();
  } catch (error) {
    console.error('Failed to fetch meteorites:', error);
    throw error;
  }
};
