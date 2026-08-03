import { api } from "../../../lib/api";
import type { RatePairingRequest, GetRandomPairingResponse } from "../types/types";

export const getRandomPairing = async () => {
    return api.get<GetRandomPairingResponse>("/pairing/random");
};

export const ratePairing = async (request: RatePairingRequest) => {
    return api.post("/pairing/rate", JSON.stringify(request));
};