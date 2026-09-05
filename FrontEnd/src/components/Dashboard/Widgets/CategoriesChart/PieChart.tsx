import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../../helpers/config';

import { PieChart as RePieChart, Pie, Cell, Tooltip, Legend } from 'recharts';

import "../widgets.css";

// const data = [
//     { name: "Dinner", value: 51 },
//     { name: "Squishies", value: 34 },
//     { name: "Salads", value: 15 }
// ];

// const COLORS = ["#FF8042", "#00C49F", "#0088FE"];

const data = [
    { name: "Dinner", value: 35 },
    { name: "Squishies", value: 28 },
    { name: "Salads", value: 12 },
    { name: "Holidays", value: 12 },
    { name: "Soups", value: 5 },
    { name: "Lunch", value: 5 },
    { name: "Dessert", value: 3 }
];


const COLORS = [
    "#FF8042", // Dinner
    "#00C49F", // Squishies
    "#0088FE", // Salads
    "#FFBB28", // Holidays (gold)
    "#A28BFF", // Soups (soft purple)
    "#4CAF50", // Lunch (deep green)
    "#FF5C7A"  // Dessert (pink-red)
];


function PieChart() {

    const CustomToolTip = ({ active, payload }) => {
        if (active && payload && payload.length) {
            return (
                <div
                    style={{
                        background: "var(--chartTooltipBackgroundColor)",
                        color: "var(--chartTooltipTextColor)",
                        padding: "6px 10px",
                        borderRadius: "6px",
                        fontSize: "14px",
                        boxShadow: "0 2px 6px rgba(0,0,0,0.4)",
                        transform: "translateY(-10px)"
                    }}
                >
                    {payload[0].name}: {payload[0].value}
                </div>
            );
        }
        return null;
    };
    return (
        <>
            <div className="chart-center no-focus">
                <RePieChart width={300} height={190}>
                    <Pie
                        data={data}
                        dataKey="value"
                        nameKey="name"
                        cx="50%"
                        cy="48%"
                        outerRadius={58}
                        isAnimationActive={false}
                    >
                        {data.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                    </Pie>

                    <Tooltip content={CustomToolTip} />
                    <Legend />
                </RePieChart>
            </div>

        </>
    );
}

export default PieChart;
