import { getRequestUrl, httpGet, httpPost } from "../common/requestApi";

const apiController = "file";

export interface FileModel {
    fileId?: number;
    fileName: string;
    mediaType: string;
    content?: ArrayBuffer
}

interface DownloadFileResponse {
    file: Blob;
}

export const upload = (base64: string, mime: string, fileName: string) => {
    var formData = new FormData();
    formData.append('file', {
        uri: base64,
        name: fileName,
        type: mime
    });

    return httpPost<number>(`${apiController}/upload`, { body: formData });
};

export const getDownloadUri = (fileId: number, relative = true) =>
    relative
        ? `${apiController}/download/${fileId}`
        : getRequestUrl(`${apiController}/download/${fileId}`);

export const download = (fileId: number) => httpGet<DownloadFileResponse>(getDownloadUri(fileId), { blob: true });
