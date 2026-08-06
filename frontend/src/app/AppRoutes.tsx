import { createBrowserRouter } from 'react-router-dom'
import AppLayout from '../layout/AppLayout'
import GonePage from '../pages/GonePage'
    
export const router = createBrowserRouter([
    {
        path: '/',
        element: <AppLayout />,
        children: [
            // {
            //     index: true,
            //     element: <HomePage />
            // },
            // {
            //     path: 'optimal-pairings',
            //     element: <OptimalPairingsPage />
            // },
            {
                index: true,
                element: <GonePage />
            }
        ]
    },
])