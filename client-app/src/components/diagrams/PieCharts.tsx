import React, { PureComponent } from 'react';
import { PieChart, Pie, Cell, Tooltip, Label, ResponsiveContainer } from 'recharts';

interface PieChartProperties {
    data: Array<Array<Object>>,
    name: string,
    value: string,
    title: string,
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
  const width = window.screen.width
  const height = window.screen.height

    return (
      <PieChart width={width} height={height}>
        {props.data.map((data, index) =>(
          <Pie
            dataKey={value}
            data={data}
            nameKey={props.name}
            cx={index >= 3 ? (15 + ((index % 3) * 33))+'%' :(15 + (index * 33)) +'%'}
            cy={index >= 3 ?  (Math.floor(index / 3)) * 600 : 200}
            outerRadius={120}
            innerRadius={60}
            labelLine={false}
            label = {renderCustomizedLabel}
          >
          <Label value={props.title} position="center" />
          {data.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
          </Pie>
        ))}
        <Tooltip />
    </PieChart>
    )
};

export default PieCharts;