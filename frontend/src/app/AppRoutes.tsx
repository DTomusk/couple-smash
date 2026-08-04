import { createBrowserRouter } from 'react-router-dom'
import AppLayout from '../layout/AppLayout'
import HomePage from '../pages/HomePage'
import OptimalPairingsPage from '../pages/OptimalPairingsPage'
    
export const router = createBrowserRouter([
    {
        path: '/',
        element: <AppLayout />,
        children: [
            {
                index: true,
                element: <HomePage />
            },
            {
                path: 'optimal-pairings',
                element: <OptimalPairingsPage />
            },
        ]
    },
])