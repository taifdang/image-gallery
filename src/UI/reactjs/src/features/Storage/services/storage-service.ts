import { apiClient } from "../../../lib/api-client";
import type { FileEntryModel } from "../types";

export async function get(id: string) {
    return apiClient.get<FileEntryModel>(`api/files/${id}`);
}

export async function getList() {
    return apiClient.get<FileEntryModel[]>("api/files");
}

export async function uploadFile(file: File) {

    const formData = new FormData();
    formData.append("Name", file.name)
    formData.append("Description", "")
    formData.append("FormFile", file)

    await apiClient.post(
        "api/files",
        formData,
        {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        })
}

export async function deleteFile(id: string) {
    return apiClient.delete(`api/files/${id}`);
}

export async function downloadFile(id: string) {
    return apiClient.get(`api/files/${id}/downloadimage`, {
        responseType: "blob"
    });
}