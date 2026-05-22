import { useEffect, useState } from 'react';
import { useNavigate, useOutletContext } from 'react-router-dom';
import { isMobileTouchDevice } from '../../../helpers/config';
import CheckAuth from '../../Account/CheckAuth';
import FavoritesMobile from './FavoritesMobile';
import ButtonGrid from '../../UserControls/ButtonGrid/ButtonGrid';
import Icon from '../../UserControls/Icons/icons';
import Loader from '../../UserControls/Loader/Loader';
import FavoritesDesktop from './FavoritesDesktop';

// ---- Types ----
interface AuthResult {
    auth: boolean;
    claims: Record<string, string>;
}

interface OutletContextType {
    setTitle: (title: string) => void;
    setBanner: (banner: string) => void;
}

function Favorites() {
    const navigate = useNavigate();
    const { setTitle, setBanner } = useOutletContext<OutletContextType>();
    const [auth, setAuth] = useState<AuthResult | null>(null);

    // Cleanup banner on unmount
    useEffect(() => {
        return () => {
            setBanner('');
        };
    }, [setBanner]);

    // Load auth
    useEffect(() => {
        async function hydrateAuth() {
            const result = await CheckAuth();
            setAuth(result);
        }
        hydrateAuth();
    }, []);

    // Redirect logic
    useEffect(() => {
        if (auth === null) return;

        if (!auth.auth) {
            navigate("/signin");
        } else {
            setTitle("Favorites");
        }
    }, [auth, navigate, setTitle]);

    

    if (auth === null) {
        return (
            <div>
                <Loader message="Loading favorites ..." />
            </div>
        );
    }

    if (!auth.auth) return null;

    return (
        <div className="page-container w-100">
            <div className={isMobileTouchDevice() ? "content-holder-mobile" : "content-holder-desktop"}>
                {isMobileTouchDevice() ? <FavoritesMobile /> : <FavoritesDesktop />}
            </div>

            {/* <ButtonGrid
                buttons={[
                    {
                        text: "Favorite",
                        url: "/favorites/favoriteinfo",
                        icon: <Icon name="add" />,
                        type: "button",
                        mobileSlot: 3,
                        desktopSlot: 5
                    }
                ]}
            /> */}
        </div>
    );
}

export default Favorites;
