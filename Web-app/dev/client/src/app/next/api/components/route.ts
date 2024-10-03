import Message from "@/types/Message";
import { NextResponse } from "next/server";
import path from "path";
import fs from "fs";

export async function GET(): Promise<NextResponse<Message[]>> {
    const filePath = path.join(process.cwd() + "/public/components/declaredComponents.json");
    let fileData = fs.readFileSync(filePath, "utf8");
    let dataArray;
    // Check if fileData is not empty and is valid JSON
    if (fileData) {
        dataArray = JSON.parse(fileData);
    }

    // If dataArray is not an array, initialize it as one
    if (!Array.isArray(dataArray)) {
        return NextResponse.json([]);
    }

    return NextResponse.json(dataArray);
}
