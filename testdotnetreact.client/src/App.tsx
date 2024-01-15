import { RouterProvider, createBrowserRouter } from "react-router-dom";
import EditTenant, { action as editAction, loader as tenantLoader } from "./routes/EditTenant.tsx";
import NewTenant, { action as newAction } from "./routes/NewTenant.tsx";
import Root, { action as deleteAction, loader as tenantsLoader } from './routes/Root.tsx';

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root/>,
    loader: tenantsLoader,
    action: deleteAction,
    children: [
        {
            index: true,
            element: <NewTenant />,
            action: newAction
        }
    ]
  },
  {
    path: "tenants/:tenantId",
    element: <EditTenant />,
    loader: tenantLoader,
    action: editAction
  }
]);

export default function App() {
    return <RouterProvider router={router} />;
  }