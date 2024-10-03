import Message from "@/types/Message";
import { URL } from "./consts";

// Get all components /api/allComponents
async function getAllComponents() {
    const res = await fetch(URL + "/api/allComponents");
    const json = (await res.json()) as Message[];
    const devices: Message[] = [];
    json.forEach((message) => {
        devices.push({
            Id: message.Id,
            Title: message.Id,
            Image: "/Lamp.png",
            Value: message.Value / 1000,
            Type: message.Type,
            Unit: message.Unit,
            Room: message.Room,
            MessageType: message.MessageType
        });
    });
    return devices;
}

async function getAllDeclaredComponents() {
    const res = await fetch(URL + "/next/api/components");
    const json = (await res.json()) as Message[];

    const devices: Message[] = [];
    json.forEach((message) => {
        devices.push({
            Id: message.Id,
            Title: message.Title as string,
            Image: message.Image as string,
            Value: message.Value / 1000,
            Type: message.Type,
            Unit: message.Unit,
            Room: message.Room,
            MessageType: message.MessageType,
            Tags: message.Tags
        });
    });
    return devices;
}

export async function addComponent(component: { Id: string; Title: string; Image: string; Tags: string[] }) {
    const res = await fetch(URL + "/next/api/component", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(component)
    });
    return await res.status;
}

export async function getAllVisibleComponents() {
    const allComps = await getAllComponents();
    const decComps = await getAllDeclaredComponents();
    const visibleComps: Message[] = [];
    allComps.forEach((comp) => {
        decComps.forEach((decComp) => {
            if (comp.Id === decComp.Id) {
                visibleComps.push({
                    Id: comp.Id,
                    Title: decComp.Title,
                    Image: decComp.Image,
                    Value: comp.Value,
                    Type: comp.Type,
                    Unit: comp.Unit,
                    Room: comp.Room,
                    MessageType: comp.MessageType,
                    Tags: decComp.Tags
                });
            }
        });
    });

    return visibleComps;
}

export async function getAllNonDeclaredComponents() {
    const allComps = await getAllComponents();
    const decComps = await getAllDeclaredComponents();
    const nonDeclared = allComps.filter((comp) => !decComps.some((decl) => decl.Id === comp.Id));

    return nonDeclared;
}

export async function getComponentById(id: string): Promise<Message> {
    const res = await fetch(URL + "/next/api/component?" + new URLSearchParams({ guid: id }));
    const resBroker = await fetch(URL + "/api/component?" + new URLSearchParams({ guid: id }));
    const json = JSON.parse((await res.json()) as string) as Message;
    const jsBk = (await resBroker.json())[0] as Message;
    const message = {
        Id: json.Id,
        Title: json.Title,
        Value: jsBk.Value / 1000,
        Type: jsBk.Type,
        Unit: jsBk.Unit,
        Room: jsBk.Room,
        MessageType: jsBk.MessageType,
        Tags: json.Tags
    };
    return message;
}

export async function getHistoryOfComponentById(id: string) {
    const res = await fetch(URL + "/api/history?" + new URLSearchParams({ guid: id }));
    const json = (await res.json()) as Message[];
    const history: Message[] = [];
    json.forEach((message) => {
        history.push({
            Id: message.Id,
            Value: message.Value / 1000,
            Type: message.Type,
            Unit: message.Unit,
            Room: message.Room,
            MessageType: message.MessageType,
            TimeStamp: message.TimeStamp
        });
    });

    return history;
}

export async function removeComponentById(id: string) {
    const res = await fetch(URL + "/next/api/component?" + new URLSearchParams({ guid: id }), {
        method: "DELETE"
    });
    return await res.status;
}

// api/component
export async function updateComponentById(message: Message) {
    const res = await fetch(URL + "/api/actors", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Id: message.Id,
            Room: message.Room,
            Value: message.Value * 1000,
            Unit: message.Unit,
            Type: message.Type,
            MessageType: "Communication"
        })
    });
    return await res.status;
}

export async function loadImages() {
    const res = await fetch(URL + "/next/api/images");
    const json = (await res.json()) as string[];
    const images: string[] = [];
    json.forEach((image) => {
        images.push(image);
    });
    return images;
}
