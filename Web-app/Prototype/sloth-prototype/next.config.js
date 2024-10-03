/** @type {import('next').NextConfig} */
const nextConfig = {
    async rewrites() {
        return [
            {
                source: "/api/:path*",
                destination: "http://localhost:5010/api/:path*"
            }
        ];
    }
};

module.exports = nextConfig;
