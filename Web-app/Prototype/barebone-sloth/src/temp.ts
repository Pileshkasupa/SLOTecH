import express from "express";

const app = express();
const port = 5010;
app.use(express.json());

app.get("/api/temperature", (req, res) => {
    res.send({ Value: 25, Unit: "C" });
});

app.post("/api/lamp", (req, res) => {
    console.log(req.body);
    res.send({ Value: "Got a GET request at /api/lamp" });
});

app.post("/api/rollo", (req, res) => {
    console.log(req.body);
    res.send({ Value: "Got a GET request at /api/rollo" });
});

app.listen(port, () => {
    console.log(`Bun app listening at http://localhost:${port}`);
});
