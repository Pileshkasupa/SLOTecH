process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

import express from "express";

const app = express();
const port = 3000;

app.get("/", async (req, res) => {
    const data = await (await fetch("http://localhost:5000/api/temperature")).text();

    res.send(data);
});

app.listen(port, () => {
    console.log(`Bun app listening at http://localhost:${port}`);
});
