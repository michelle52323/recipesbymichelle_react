

export const FetchMockClaims = async () => {
    const API_BASE = getApiBaseUrl();
alert("0");
    try {
        const response = await fetch(`${API_BASE}/api/mock/claims`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include' // optional for dev parity
        });

        if (!response.ok) {
            alert("1");
            throw new Error(`Mock claims fetch failed: ${response.status}`);
        }

        const claims = await response.json();
        console.log('Mock claims:', claims);

        return claims;
    } catch (error) {
        console.error('Error fetching mock claims:', error);
        alert("2");
        return null;
    }
};