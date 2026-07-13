import { useEffect, useState } from 'react';
import { useNavigate, useLocation, useOutletContext } from 'react-router-dom';
import Icon from '../UserControls/Icons/icons';
import AddRecipeActionsMenu from '../UserControls/SubMenus/MyRecipes/AddRecipeActionsMenu';

import CheckAuth from '../../components/Account/CheckAuth';
import Loader from '../UserControls/Loader/Loader';
import './dashboard.css';

// ---- Types ----
interface AuthResult {
    auth: boolean;
    claims: Record<string, any>;
}

interface OutletContextType {
    setTitle: (title: string) => void;
    setBanner: (banner: string) => void;
}

function Dashboard() {
    const navigate = useNavigate();
    const location = useLocation();
    const { setTitle, setBanner } = useOutletContext<OutletContextType>();

    const [auth, setAuth] = useState<AuthResult | null>(null);
    const [isGuest, setIsGuest] = useState<boolean>(false);

    const [showOverview, setShowOverview] = useState<boolean>(false);

    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isClosing, setIsClosing] = useState(false);

    const closeMenu = () => {
        setIsClosing(true);

        setTimeout(() => {
            setIsMenuOpen(false);
            setIsClosing(false);
        }, 150);
    };

    // Load auth
    useEffect(() => {
        async function hydrateAuth() {
            const result = await CheckAuth();
            setAuth(result);
        }
        hydrateAuth();
    }, []);

    // Redirect + hydrate claims
    useEffect(() => {
        if (auth === null) return;

        if (!auth.auth) {
            navigate("/signin");
        } else {
            setTitle("Welcome, " + auth.claims.FirstName);
        }

        if (
            auth.claims["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ===
            "Guest User"
        ) {
            setIsGuest(true);
        }
    }, [auth, navigate, setTitle]);

    // Cleanup banner on unmount
    useEffect(() => {
        return () => {
            setBanner('');
        };
    }, [setBanner]);

    // Handle banner passed via navigation state
    useEffect(() => {
        if (location.state && (location.state as any).banner) {
            const banner = (location.state as any).banner;
            setBanner(banner);

            navigate(location.pathname, {
                replace: true,
                state: {},
            });
        }
    }, [location.state, location.pathname, navigate, setBanner]);

    if (auth === null) {
        return (
            <div>
                <Loader message="Loading dashboard ..." />
            </div>
        );
    }

    if (!auth.auth) return null;

    return (
        <div className="page-container w-100 pt-3">
            <div className="content-holder-desktop">
                <div className="content-inner-desktop">
                    {isGuest && (
                        <div className="guest-mode-notice">
                            You’re currently using <strong>Guest Mode</strong>.
                            This temporary account gives you full access to explore the app for up to <strong>10 days</strong>.
                            After that period, your guest access will expire and all guest data will be removed.
                            To continue using the app beyond the trial window — and to keep your data —
                            please register a full account.
                            Some features may be limited while in Guest Mode.
                        </div>
                    )}


                    <div className="dashboard-container row g-3">

                        <div className="dashboard-item col-12 col-md-4 text-center">
                            <div
                                className="dashboard-link-inner"
                                onClick={() => navigate("/Recipes/MyRecipes")}
                                style={{ cursor: "pointer", display: "inline-block" }}
                            >
                                <span className="w-100">My Recipes</span>
                            </div>
                        </div>

                        <div className="dashboard-item col-12 col-md-4 text-center">
                            <div
                                className="dashboard-link-inner"
                                onClick={() => setIsMenuOpen(true)}
                                style={{ cursor: "pointer", display: "inline-block" }}
                            >
                                <span className="w-100">Add New Recipe</span>
                            </div>
                        </div>

                        <div className="dashboard-item col-12 col-md-4 text-center">
                            <div
                                className="dashboard-link-inner"
                                onClick={() => navigate("/Recipes/Search")}
                                style={{ cursor: "pointer", display: "inline-block" }}
                            >
                                <span className="w-100">Search Recipes</span>
                            </div>
                        </div>

                        <div className="dashboard-item col-12 col-md-4 text-center">
                            <div
                                className="dashboard-link-inner"
                                onClick={() => navigate("/Account/Profile")}
                                style={{ cursor: "pointer", display: "inline-block" }}
                            >
                                <span className="w-100">Profile</span>
                            </div>
                        </div>

                        {!isGuest && (
                            <div className="dashboard-item col-12 col-md-4 text-center">
                                <div
                                    className="dashboard-link-inner"
                                    onClick={() => navigate("/Account/ChangePassword")}
                                    style={{ cursor: "pointer", display: "inline-block" }}
                                >
                                    <span className="w-100">Change Password</span>
                                </div>
                            </div>
                        )}


                        <div className="dashboard-item col-12 col-md-4 text-center">
                            <div
                                className="dashboard-link-inner"
                                onClick={() => navigate("/Account/Themes")}
                                style={{ cursor: "pointer", display: "inline-block" }}
                            >
                                <span className="w-100">Themes</span>
                            </div>
                        </div>

                    </div>

                    {isMenuOpen && (
                        <>
                            <div
                                className="mobile-menu-backdrop"
                                onClick={closeMenu}
                                style={{
                                    position: 'fixed',
                                    inset: 0,
                                    background: 'rgba(0,0,0,0.4)',
                                    zIndex: 9998
                                }}
                            ></div>

                            <AddRecipeActionsMenu
                                navigate={(path: string) => navigate(path)}
                                closeMenu={closeMenu}
                                isClosing={isClosing}
                            />
                        </>
                    )}

                </div>
            </div>
        </div>
    );
}

export default Dashboard;
