/// <reference lib="dom" />
/// <reference lib="dom.iterable" />
"use client";
import { useState } from "react";

export type Message = {
    Id: string;
    Room: string;
    Value: number;
    Unit: string;
    Type: string;
    MessageType: string;
};

export default function Home() {
    const [lampState, setLampState] = useState(false);
    const [temperature, setTemperature] = useState([0]);
    const [rolloState, setRolloState] = useState(false);
    const interval = setInterval(() => {
        fetch("http://10.140.1.122:3000/api/temperature", {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        }).then((res) =>
            res.json().then((data) => {
                if (temperature[0] === 0) setTemperature([data.Value]);
                else if (temperature[0] === data.Value) return;
                else setTemperature((prevTemp) => [data.Value, ...prevTemp]);
            })
        );
    }, 5000);

    const LampToggle = (e: any) => {
        const lamp = document.getElementById("lamp");
        if (lampState === false && lamp) {
            //Post Request to api Lamp
            fetch("http://10.140.1.122:3000/api/lamp", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ Id: "Lamp", Room: "Living", Value: 0, Unit: "boolean", Type: "lamp", MessageType: "action" } as Message)
            }).then((res) => console.log(res));

            setLampState(true);
            lamp.classList.remove("turnOn");
            lamp.classList.add("turnOff");
        } else {
            if (lamp) {
                fetch("http://10.140.1.122:3000/api/lamp", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        jsonObject: { Id: "Lamp", Room: "Living", Value: 1, Unit: "boolean", Type: "lamp", MessageType: "action" }
                    })
                }).then((res) => console.log(res));
                setLampState(false);
                lamp.classList.remove("turnOff");
                lamp.classList.add("turnOn");
            }
        }
    };

    return (
        <div className="h-screen w-screen grid grid-cols-3 gap-4 content-center bg-[#14151a] justify-items-center">
            <div
                id="lamp"
                className=" font-system-ui font-bold rounded-3xl border-4 border-[#f472b6] p-8 bg-[#282a36] w-[27em] col-start-2 cursor-pointer  turnOn"
                onClick={LampToggle}
            >
                <h1 className="text-9xl transition-all  text-center select-none">Lamp</h1>
            </div>
            <div>
                <button
                    className="font-system-ui font-bold rounded-3xl border-4 p-8 bg-[#282a36] w-[27em] col-start-2 cursor-pointer text-white"
                    onClick={() => {
                        if (rolloState) {
                            fetch("http://localhost:3000/api/rollo", {
                                method: "POST",
                                headers: {
                                    "Content-Type": "application/json"
                                },
                                body: JSON.stringify({
                                    Id: "Rollo",
                                    Room: "Living",
                                    Value: 1,
                                    Unit: "boolean",
                                    Type: "lamp",
                                    MessageType: "action"
                                } as Message)
                            }).then((res) => res.json().then((data) => console.log(data.Value)));
                            setRolloState(false);
                        } else {
                            fetch("http://localhost:3000/api/rollo", {
                                method: "POST",
                                headers: {
                                    "Content-Type": "application/json"
                                },
                                body: JSON.stringify({
                                    Id: "Rollo",
                                    Room: "Living",
                                    Value: 0,
                                    Unit: "boolean",
                                    Type: "lamp",
                                    MessageType: "action"
                                } as Message)
                            }).then((res) => res.json().then((data) => console.log(data.Value)));
                            setRolloState(true);
                        }
                    }}
                >
                    Rollo
                </button>
            </div>
            <div className="textbox text-white col-start-2 border-4 rounded-xl w-[27em] h-[8em] border-[#1b1c22] overflow-auto">
                <ul id="infoList">
                    {temperature.map((temp, id) => (
                        <li key={id}>{temp.toString()}</li>
                    ))}
                </ul>
            </div>
        </div>
    );
}
