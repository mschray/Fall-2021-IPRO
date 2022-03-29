import React, { PureComponent } from 'react';
import { PieChart, Pie, Sector, Cell, ResponsiveContainer, Tooltip } from 'recharts';

interface PieChartProperties {
    data: Array<Object>,

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
    const data = props.data
    return (
        <PieChart width={400} height={400}>
            <Pie
              dataKey="value"
              data={props.data}
              cx={200}
              cy={200}
              outerRadius={80}
              labelLine={false}
              fill="#8884d8"
              label = {renderCustomizedLabel}
            >
            {data.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
            ))}
            </Pie>
            <Tooltip />
        </PieChart>
    );
};

export default PieCharts;