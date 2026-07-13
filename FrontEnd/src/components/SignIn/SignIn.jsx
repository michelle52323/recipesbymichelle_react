import { useOutletContext } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getApiBaseUrl, isMobileTouchDevice } from '../../helpers/config';
import PasswordInput from '../../components/UserControls/PasswordToggle/PasswordToggle';
import SignInLoader from '../UserControls/SignInLoader/SignInLoader';


function SignInForm() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [isLoadingGuest, setIsLoadingGuest] = useState(false);
    const [hasStarterKit, setHasStarterKit] = useState(null);
    const [errors, setErrors] = useState({});

    const API_BASE = getApiBaseUrl();

    const handleSubmit = (e) => {
        setBanner('');
        e.preventDefault();
        const newErrors = {};
        if (!username.trim()) newErrors.username = 'Username is required';
        if (!password.trim()) newErrors.password = 'Password is required';
        setErrors(newErrors);

        if (Object.keys(newErrors).length === 0) {
            (async () => {
                try {
                    setIsLoading(true);

                    const response = await fetch(API_BASE + `/api/signin/sign-in`, {
                        method: 'POST',
                        credentials: 'include',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ username, password })
                    });

                    const data = await response.json();

                    if (data.success) {
                        //console.log("SignIn API result:", JSON.stringify(data, null, 2));




                        if (!data.hasMeasurementSystem) {
                            window.location.href = '/account/selectmeasurementsystem';
                            return;
                        }

                        localStorage.setItem('authToken', data.token); // optional
                        window.location.href = '/dashboard';
                    } else {
                        setIsLoading(false);
                        setHasStarterKit(null);
                        if (data.status = 429) {
                            setBanner(data.failureReason);
                        }
                        else if (data.status = 401) {
                            setBanner('Invalid username or password');
                        }

                    }
                } catch (err) {
                    setIsLoading(false);
                    setHasStarterKit(null);
                    //console.error('Login error:', err);
                    setBanner('Something went wrong');


                }
            })();
        }
    };

    const { setTitle, setBanner } = useOutletContext();
    const navigate = useNavigate();

    useEffect(() => {
        setTitle('Sign In');
    }, []);

    const cardClass = isMobileTouchDevice() ? "mobile-card" : "narrow-card";

    const handleGuestAccess = async () => {
        setIsLoadingGuest(true);
        let deviceId = localStorage.getItem("deviceId");

        const payload = {
            deviceId: deviceId ? deviceId : null
        };

        try {
            const response = await fetch(API_BASE + "/api/GuestAccount/access", {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            const data = await response.json();

            if (!response.ok) {
                const text = await response.text(); // <-- raw error
                //console.error("Backend error:", text);
                setBanner("Guest access failed: ");
                setIsLoadingGuest(false);
                return;
            }


            if (!data.success) {
                setIsLoadingGuest(false);
                setBanner("Guest access failed");
                return;
            }

            if (!data.accessGranted) {
                setIsLoadingGuest(false);
                setBanner("Guest access expired. Please register an account to continue using.");
                return;
            }

            // If backend created a new guest user, it returns a deviceId
            if (data.deviceId) {
                localStorage.setItem("deviceId", data.deviceId);
            }

            // Auth cookie is already set by SignInUserAsync
            window.location.href = "/dashboard";

        } catch (err) {
            setIsLoadingGuest(false);
            //console.error("Guest access error:", err);
            setBanner("Something went wrong");
        }
    };

    if (isLoadingGuest) {
        return <SignInLoader isGuest={true} />;
    }

    if (isLoading && hasStarterKit === null) {
        return <SignInLoader />;
    }

    if (isLoading) {
        return <SignInLoader hasStarterKit={hasStarterKit} />;
    }

    return (
        <form onSubmit={handleSubmit}>
            <main
                className={`${cardClass} narrow-card content-holder-narrow`}
                style={{ height: '324px' }}
            >

                <div className='form-row'>
                    <label>Username</label>
                    <span className='required'>*</span>
                    <br />
                    <div className='form-element'>
                        <input
                            type="text"
                            className="form-control textbox textbox-text textbox-large"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                    </div>

                    {errors.username && <div className='error-message'>{errors.username}</div>}
                </div>

                <div className='form-row'>
                    <label>Password</label>
                    <span className='required'>*</span>
                    <br />
                    <div className='form-element'>
                        <PasswordInput
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            name="password"
                        />
                    </div>

                    {errors.password && <div className='error-message'>{errors.password}</div>}
                </div>
                <div className='form-row d-flex justify-content-center align-items-center'>
                    <button type="submit" className='button'>Sign In</button>
                </div>
                <div className='d-flex justify-content-center align-items-center'>
                    <button type="button" className='button' onClick={handleGuestAccess}>Guest Access</button>
                </div>

            </main>

        </form>

    );
}

export default SignInForm;