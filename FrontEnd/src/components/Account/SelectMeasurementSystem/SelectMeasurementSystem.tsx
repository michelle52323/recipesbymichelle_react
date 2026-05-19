import { useState, useEffect } from 'react';
import { useNavigate, useOutletContext, useLocation } from 'react-router-dom';
import { getApiBaseUrl, isMobileTouchDevice } from '../../../helpers/config';
import type { LayoutContext } from '../../Layout';
import { Dropdown } from '../../UserControls/Dropdown/Dropdown';
import TextboxUnique from '../../UserControls/TextboxUnique/TextboxUnique';
import { checkEmailForProfile } from '../../UserControls/TextboxUnique/uniquevalidation';
import CheckAuth from '../CheckAuth';
import ChangePassword from '../../UserControls/ChangePassword/ChangePassword';
import { ChangePasswordModel } from '../../UserControls/ChangePassword/ChangePassword';
import Loader from '../../UserControls/Loader/Loader';

const API_BASE = getApiBaseUrl();


function SelectMeasurementSystem() {
    const { setTitle, setBanner } = useOutletContext();
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [errors, setErrors] = useState({} as any);
    const [measurementSystem, setMeasurementSystem] = useState<"Imperial" | "Metric" | null>(null);
    setTitle('Measurement System');

    const cardClass = isMobileTouchDevice() ? "mobile-card" : "narrow-card";

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const newErrors: any = {};

        if (!measurementSystem) {
            newErrors.measurementSystem = "Please select a measurement system.";
        }

        setErrors(newErrors);

        if (Object.keys(newErrors).length > 0) {
            return; // stop submission
        }

        // continue to API call...
        try {
            const response = await fetch(`${API_BASE}/api/users/update-measurement-system`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include",
                body: JSON.stringify({
                    measurementSystem: measurementSystem
                })
            });

            if (!response.ok) {
                throw new Error("Failed to update measurement system");
            }

            setBanner('Measurement system updated successfully!');
            navigate('/dashboard');

            // success handling here (redirect, refresh auth, etc.)

        } catch (err) {
            console.error(err);
            setErrors({
                measurementSystem: "Something went wrong. Please try again."
            });
        }


    };

    return (
        <form onSubmit={handleSubmit}>
            <main className={`${cardClass} narrow-card content-holder-narrow`}
                style={{ height: '284px' }}
            >



                {/* MEASUREMENT SYSTEM */}

                <div className="page-item col-12 " style={{ height: "157px" }}>
                    <label>Select Measurement System</label>
                    <div className="form-element">
                        <div className="radio-holder-vertical">
                            <ul>
                                <li>
                                    <input
                                        type="radio"
                                        id="measurement-system-imperial"
                                        name="measurementSystem"
                                        value="Imperial"
                                        checked={measurementSystem === "Imperial"}
                                        onChange={() => setMeasurementSystem("Imperial")}
                                    />
                                    <div className="check"></div>
                                    <label htmlFor="measurement-system-imperial">Imperial (U.S.)</label>
                                </li>

                                <li>
                                    <input
                                        type="radio"
                                        id="measurement-system-metric"
                                        name="measurementSystem"
                                        value="Metric"
                                        checked={measurementSystem === "Metric"}
                                        onChange={() => setMeasurementSystem("Metric")}
                                    />
                                    <div className="check"></div>
                                    <label htmlFor="measurement-system-metric">Metric</label>
                                </li>
                            </ul>

                            <span className="ps-3 d-block">
                                Used for measurement units
                            </span>
                        </div>
                        {errors.measurementSystem && (
                            <div className="error-message">{errors.measurementSystem}</div>
                        )}

                    </div>

                </div>

                {/* SUBMIT */}
                <div className="form-row d-flex justify-content-center align-items-center">
                    <button type="submit" className="button">Submit</button>
                </div>

            </main>
        </form>

    );

}

export default SelectMeasurementSystem;