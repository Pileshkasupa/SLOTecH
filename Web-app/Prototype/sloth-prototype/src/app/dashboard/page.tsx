// `app` directory

export async function getTemperature() {
    const res = await fetch(`http://localhost:5010/api/temperature`, { cache: "no-store" });
    const temp = await res.json();

    return temp;
}

export default async function Dashboard() {
    const list = <ul id="infoList">{(await getTemperature()).Value}</ul>;

    return list;
}
