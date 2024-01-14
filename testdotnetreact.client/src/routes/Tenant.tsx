import { Form, Params, useLoaderData } from "react-router-dom";
import { getTenant } from "../tenants";

export type Tenant = {
    id: string;
    name: string;
    country: string;
}

export default function Tenant() {
  const { tenant } = useLoaderData() as { tenant: Tenant };

  return (
    <Form method="put" id="tenant-form">
      <label>
        <span>Name</span>
        <input
          placeholder="Name"
          aria-label="Name"
          type="text"
          name="name"
          defaultValue={tenant.name}
        />
      </label>
      <label>
        <span>Country</span>
        <input
          placeholder="Country"
          aria-label="Country"
          type="text"
          name="country"
          defaultValue={tenant.country}
        />
      </label>
      <p>
        <button type="submit">Save</button>
        <button type="button">Cancel</button>
      </p>
    </Form>
  );
}

export async function loader({ params }: {params: Params<string>}) {
    const tenant = await getTenant(params.tenantId!)
    return { tenant };
}