import { useEffect, useState, useRef } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isDevUseMockLogin, isMobileTouchDevice, getApiBaseUrl } from '../../../../helpers/config';
import PieChart from "../CategoriesChart/PieChart";
import BarChartTopFive from "../CategoriesChart/BarChart";

import "../widgets.css";

type CategoriesChartProps = {
    closeMenu: () => void;
    isClosing: boolean;
    setIsClosing: React.Dispatch<React.SetStateAction<boolean>>;
    isMenuOpen: boolean;
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

export default function CategoriesChart({
    closeMenu,
    isClosing,
    setIsClosing,
    isMenuOpen,
    setIsMenuOpen
}: CategoriesChartProps) {
    return (
        <div className="widget-container">
            <h3 className="widget-title">Your Cooking Profile</h3>
            <div className="widget-text widget-text-bold">
                <BarChartTopFive
                    closeMenu={closeMenu}
                    isClosing={isClosing}
                    setIsClosing={setIsClosing}
                    isMenuOpen={isMenuOpen}
                    setIsMenuOpen={setIsMenuOpen}
                />
            </div>
        </div>
    );
}