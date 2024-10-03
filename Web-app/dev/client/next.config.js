/** @type {import('next').NextConfig} */
const nextConfig = {
    async rewrites() {
        return [
            {
                source: "/api/:path*",
                destination: "http://localhost:5010/api/:path*"
            }
        ];
    },
    async redirects() {
        return [
            {
                source: "/devices",
                destination: "/",
                permanent: true
            }
        ];
    }
};

module.exports = nextConfig;
