export type RatePairingRequest = {
    pairingId: string;
    rating: number;
};

export type GetRandomPairingResponse = {
    pairingId: string;
    firstMemberName: string;
    secondMemberName: string;
}