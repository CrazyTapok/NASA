import { useEffect, useState } from 'react';
import { MeteoriteGroup } from '../api/model/meteoriteGroup';
import { getMeteorites, getUniqueRecClass, getUniqueYears } from '../api/api';
import { ControlPanel } from './ui/controlPanel';
import { TableData } from './ui/tableData';
import { message } from 'antd';

import './ui/_app.scss';
import titleImage from '../shared/style/title.png';

function App() {
  const [years, setYears] = useState<number[]>([]);
  const [recClasses, setRecClasses] = useState<string[]>([]);
  const [meteorites, setMeteorites] = useState<MeteoriteGroup[]>([]);
  const [startYear, setStartYear] = useState<number | null>(null);
  const [endYear, setEndYear] = useState<number | null>(null);
  const [recClass, setRecClass] = useState<string>('');
  const [namePart, setNamePart] = useState<string>('');
  const [sortBy, setSortBy] = useState<string>('');
  const [ascending, setAscending] = useState<boolean>(true);

  useEffect(() => {
    getUniqueYears()
      .then((data: number[]) => setYears(data))
      .catch((error) => {
        message.error('Failed to fetch unique years');
        console.error(error);
      });

    getUniqueRecClass()
      .then((data: string[]) => setRecClasses(data))
      .catch((error) => {
        message.error('Failed to fetch unique rec classes');
        console.error(error);
      });
  }, []);

  useEffect(() => {
    searchMeteorites();
  }, [sortBy, ascending]);

  const searchMeteorites = () => {
    if (endYear !== null && startYear !== null) {
      if (startYear > endYear) {
        message.error('Start year cannot be greater than end year');
        return;
      }
    } else if (startYear !== null && endYear === null) {
      message.error('Invalid time interval');
      return;
    } else if (startYear === null && endYear !== null) {
      message.error('Invalid time interval');
      return;
    }

    getMeteorites(startYear, endYear, recClass, namePart, sortBy, ascending)
      .then((data: MeteoriteGroup[]) => {
        setMeteorites(data);
      })
      .catch((error) => {
        message.error('Failed to fetch meteorites');
        console.error(error);
      });
  };

  const setParamFilter = (startYear: number | null, endYear: number | null, recClass: string, namePart: string) => {
    setStartYear(startYear);
    setEndYear(endYear);
    setRecClass(recClass);
    setNamePart(namePart);
  };

  const setParamSort = (column: string) => {
    if (sortBy === column) {
      if (ascending) {
        setAscending(false);
      } else {
        setSortBy('');
        setAscending(true);
      }
    } else {
      setSortBy(column);
      setAscending(true);
    }
  };

  return (
    <>
      <div className="main">
        <div className="main-img">
          <img src={titleImage} alt="Title" />
        </div>
        <ControlPanel
          years={years}
          recClasses={recClasses}
          onSetParamFilter={setParamFilter}
          onGetMeteorites={searchMeteorites}
        />
        <TableData meteorites={meteorites} onSort={setParamSort} />
      </div>
    </>
  );
}

export default App;
