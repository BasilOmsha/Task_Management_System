import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx"
import { Cell, Pie, PieChart, ResponsiveContainer, Tooltip } from 'recharts'

interface UserRolesChartProps {
    data: { name: string; value: number }[]
}

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8']

export function UserRolesChart({ data }: UserRolesChartProps) {
    return (
        <Card>
            <CardHeader>
                <CardTitle>User Roles Distribution</CardTitle>
            </CardHeader>
            <CardContent>
                <ResponsiveContainer height={250} width="100%">
                    <PieChart>
                        <Pie
                            cx="50%"
                            cy="50%"
                            data={data}
                            dataKey="value"
                            fill="#8884d8"
                            label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
                            labelLine={false}
                            outerRadius={80}
                        >
                            {data.map((entry, index) => (
                                <Cell fill={COLORS[index % COLORS.length]} key={`cell-${index}`} />
                            ))}
                        </Pie>
                        <Tooltip />
                    </PieChart>
                </ResponsiveContainer>
            </CardContent>
        </Card>
    )
}

