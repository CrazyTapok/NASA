import { Table } from 'antd';
import { ColumnsType } from 'antd/es/table';
import { MeteoriteGroup } from '../../api/model/meteoriteGroup';

export const TableData = ({
  meteorites,
  onSort,
}: {
  meteorites: MeteoriteGroup[];
  onSort: (column: string) => void;
}) => {
  const columns: ColumnsType<MeteoriteGroup> = [
    {
      title: 'Year',
      dataIndex: 'year',
      key: 'year',
      sorter: true,
      onHeaderCell: () => ({
        onClick: () => onSort('Year'),
      }),
    },
    {
      title: 'Count',
      dataIndex: 'count',
      key: 'count',
      sorter: true,
      onHeaderCell: () => ({
        onClick: () => onSort('Count'),
      }),
    },
    {
      title: 'TotalMass',
      dataIndex: 'totalMass',
      key: 'totalMass',
      sorter: true,
      onHeaderCell: () => ({
        onClick: () => onSort('TotalMass'),
      }),
    },
  ];

  return (
    <Table
      columns={columns}
      dataSource={meteorites}
      rowKey={(record) => `${record.year}-${record.count}-${record.totalMass}`}
    />
  );
};
