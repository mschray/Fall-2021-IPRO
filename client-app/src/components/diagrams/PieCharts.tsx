import React, { PureComponent } from 'react';
import { PieChart, Pie, Cell, Tooltip } from 'recharts';

interface PieChartProperties {
    data: Array<Object>,
    name: string,
    value: string
}
const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({ cx, cy, midAngle, innerRadius, outerRadius, percent, index, payload }: any) => {
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
  const x = cx + radius * Math.cos(-midAngle * RADIAN);
  const y = cy + radius * Math.sin(-midAngle * RADIAN);

  return (
    <text x={x} y={y} fill="white" textAnchor={x > cx ? 'start' : 'end'} dominantBaseline="central">
      {`${(percent * 100).toFixed(0)}%`}
    </text>
  );
};

const PieCharts: React.FC<PieChartProperties> = (props) => {
  const value = props.value
    return (
        <PieChart width={400} height={400}>
            <Pie
              dataKey={value}
              data={props.data}
              nameKey={props.name}
              cx={200}
              cy={200}
              outerRadius={80}
              labelLine={false}
              label = {renderCustomizedLabel}
            >
            {props.data.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
            ))}
            </Pie>
            <Tooltip />
        </PieChart>
    );
};

export default PieCharts;