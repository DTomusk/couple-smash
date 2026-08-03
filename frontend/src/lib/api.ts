import { apiFetch } from "./client";

// These are just helper functions that make calling code more legible
export const api = {
    get: <T>(url: string) =>
        apiFetch<T>(url),

    post: <T>(url: string, body?: BodyInit) =>
        apiFetch<T>(url, {
            method: 'POST',
            body,
        }),

    put: <T>(url: string, body?: BodyInit) =>
        apiFetch<T>(url, {
            method: 'PUT',
            body,
        }),

    delete: <T>(url: string) =>
        apiFetch<T>(url, {
            method: 'DELETE',
        }),
};