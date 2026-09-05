import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../../helpers/config';
import PieChart from "../CategoriesChart/PieChart";
import BarChartTopFive from "../CategoriesChart/BarChart";

import "../widgets.css";

function CategoriesChart() {

    

    return (
        <div className="widget-container">
            <h3 className="widget-title">You're Cooking Profile</h3>
            <div className="widget-text widget-text-bold"><BarChartTopFive/></div>
        </div>
    );
}

export default CategoriesChart;
