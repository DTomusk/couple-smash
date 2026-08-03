import { useGetRandomPairing } from "../features/pairings/hooks/usePairing";

export default function HomePage() {
    // Start by loading a random pairing
    const { data: pairing, isLoading, isError } = useGetRandomPairing();

    if (isLoading) {
        return (
            <div>Loading...</div>
        )
    }
    
    if (isError) {
        return (
            <div>Error loading pairing.</div>
        )
    }

    return (
        <>
            <h1>Random Pairing</h1>
            <p>Pairing ID: {pairing?.pairingId}</p>
            <p>First Member: {pairing?.firstMemberName}</p>
            <p>Second Member: {pairing?.secondMemberName}</p>
        </>
    )
}