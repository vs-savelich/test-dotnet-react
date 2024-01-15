import { Form, Params } from "react-router-dom";
import { TenantData, createTenant, getTenant } from "../tenants";

export default function NewTenant() {
  return (
    <Form method="post" id="new-tenant">
        <label>
            <span>Name</span>
            <input
            placeholder="Name"
            aria-label="Name"
            type="text"
            name="name"
            />
        </label>
        <label>
            <span>Country</span>
            <input
            placeholder="Country"
            aria-label="Country"
            type="text"
            name="country"
            />
        </label>
        <button type="submit">Add</button>
    </Form>
  );
}

export async function loader({ params }: {params: Params<string>}) {
    const tenant = await getTenant(params.tenantId!)
    return { tenant };
}

export async function action({ request }: {request: Request}) {
  const formData = await request.formData();
  const tenant = await createTenant(Object.fromEntries(formData) as TenantData);
  return { tenant };
}