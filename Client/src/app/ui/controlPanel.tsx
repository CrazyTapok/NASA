import { useEffect, useState } from 'react';
import { Button, Input, Select } from 'antd';

const { Option } = Select;
const { Search } = Input;

export const ControlPanel = ({
  years,
  recClasses,
  onSetParamFilter,
  onGetMeteorites,
}: {
  years: number[];
  recClasses: string[];
  onSetParamFilter: (startYear: number | null, endYear: number | null, recClass: string, namePart: string) => void;
  onGetMeteorites: () => void;
}) => {
  const [startYear, setStartYear] = useState<number | null>(null);
  const [endYear, setEndYear] = useState<number | null>(null);
  const [recClass, setRecClass] = useState<string>('');
  const [namePart, setNamePart] = useState<string>('');

  useEffect(() => {
    onSetParamFilter(startYear, endYear, recClass, namePart);
  }, [startYear, endYear, recClass, namePart]);

  const handleClear = () => {
    setStartYear(null);
    setEndYear(null);
    setRecClass('');
    setNamePart('');
  };

  return (
    <div className="main-panel">
      <div className="main-panel-filter">
        <Select
          className="main-panel-filter-select"
          placeholder="Year From"
          value={startYear !== null ? startYear : undefined}
          onChange={(value) => {
            setStartYear(Number(value));
          }}
        >
          {years.map((year) => (
            <Option key={year} value={year}>
              {year}
            </Option>
          ))}
        </Select>
        <Select
          className="main-panel-filter-select"
          placeholder="Year To"
          value={endYear !== null ? endYear : undefined}
          onChange={(value) => setEndYear(Number(value))}
        >
          {years.map((year) => (
            <Option key={year} value={year}>
              {year}
            </Option>
          ))}
        </Select>
        <Select
          className="main-panel-filter-select"
          placeholder="Select Group"
          value={recClass || undefined}
          onChange={(value) => setRecClass(value)}
        >
          {recClasses.map((recClass) => (
            <Option key={recClass} value={recClass}>
              {recClass}
            </Option>
          ))}
        </Select>
        <Button type="primary" onClick={onGetMeteorites}>
          Apply Filter
        </Button>
        <Button danger onClick={handleClear}>
          Clear
        </Button>
      </div>

      <Search
        className="main-panel-search"
        placeholder="Search"
        value={namePart}
        onChange={(e) => setNamePart(e.target.value)}
        onSearch={onGetMeteorites}
        allowClear
        enterButton="Search"
      />
    </div>
  );
};
