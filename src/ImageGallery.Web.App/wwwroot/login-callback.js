export function getAccessToken() {
    const hash = window.location.hash.substring(1);
    const params = new URLSearchParams(hash);
    const token = params.get('access_token');
    window.location.hash = '';
    return token;
}
