import { NextResponse } from "next/server";
import path from "path";
import fs from "fs";

export async function GET() {
    const directoryPath = path.join(process.cwd(), "public");
    const files = fs.readdirSync(directoryPath);

    // Filter out only .png files
    const pngFiles = files.filter((file) => file.endsWith(".png"));
    const filePaths = pngFiles.map((file) => path.join("/", file));

    return NextResponse.json(filePaths);
}
