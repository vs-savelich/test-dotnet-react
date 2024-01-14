import React from 'react';
import ReactDOM from 'react-dom/client';
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import './index.css';
import App, { loader as tenantsLoader } from './routes/App.tsx';
import Tenant, { loader as tenantLoader } from "./routes/Tenant.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App/>,
    loader: tenantsLoader
  },
  {
    path: "tenants/:tenantId",
    element: <Tenant />,
    loader: tenantLoader

  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>,
)
