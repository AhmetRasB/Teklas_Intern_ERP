import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import HomePageOne from "./pages/HomePageOne";
import HomePageTwo from "./pages/HomePageTwo";
import HomePageThree from "./pages/HomePageThree";
import HomePageFour from "./pages/HomePageFour";
import HomePageFive from "./pages/HomePageFive";
import HomePageSix from "./pages/HomePageSix";
import HomePageSeven from "./pages/HomePageSeven";
import EmailPage from "./pages/EmailPage";
import AddUserPage from "./pages/AddUserPage";
import AlertPage from "./pages/AlertPage";
import AssignRolePage from "./pages/AssignRolePage";
import AvatarPage from "./pages/AvatarPage";
import BadgesPage from "./pages/BadgesPage";
import ButtonPage from "./pages/ButtonPage";
import CalendarMainPage from "./pages/CalendarMainPage";
import CardPage from "./pages/CardPage";
import CarouselPage from "./pages/CarouselPage";
import ChatEmptyPage from "./pages/ChatEmptyPage";
import ChatMessagePage from "./pages/ChatMessagePage";
import ChatProfilePage from "./pages/ChatProfilePage";
import CodeGeneratorNewPage from "./pages/CodeGeneratorNewPage";
import CodeGeneratorPage from "./pages/CodeGeneratorPage";
import ColorsPage from "./pages/ColorsPage";
import ColumnChartPage from "./pages/ColumnChartPage";
import CompanyPage from "./pages/CompanyPage";
import CurrenciesPage from "./pages/CurrenciesPage";
import DropdownPage from "./pages/DropdownPage";
import ErrorPage from "./pages/ErrorPage";
import FaqPage from "./pages/FaqPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import FormLayoutPage from "./pages/FormLayoutPage";
import FormValidationPage from "./pages/FormValidationPage";
import FormPage from "./pages/FormPage";
import GalleryPage from "./pages/GalleryPage";
import ImageGeneratorPage from "./pages/ImageGeneratorPage";
import ImageUploadPage from "./pages/ImageUploadPage";
import InvoiceAddPage from "./pages/InvoiceAddPage";
import InvoiceEditPage from "./pages/InvoiceEditPage";
import InvoiceListPage from "./pages/InvoiceListPage";
import InvoicePreviewPage from "./pages/InvoicePreviewPage";
import KanbanPage from "./pages/KanbanPage";
import LanguagePage from "./pages/LanguagePage";
import LineChartPage from "./pages/LineChartPage";
import ListPage from "./pages/ListPage";
import MarketplaceDetailsPage from "./pages/MarketplaceDetailsPage";
import MarketplacePage from "./pages/MarketplacePage";
import NotificationAlertPage from "./pages/NotificationAlertPage";
import NotificationPage from "./pages/NotificationPage";
import PaginationPage from "./pages/PaginationPage";
import PaymentGatewayPage from "./pages/PaymentGatewayPage";
import PieChartPage from "./pages/PieChartPage";
import PortfolioPage from "./pages/PortfolioPage";
import PricingPage from "./pages/PricingPage";
import ProgressPage from "./pages/ProgressPage";
import RadioPage from "./pages/RadioPage";
import RoleAccessPage from "./pages/RoleAccessPage";
import SignInPage from "./pages/SignInPage";
import SignUpPage from "./pages/SignUpPage";
import StarRatingPage from "./pages/StarRatingPage";
import StarredPage from "./pages/StarredPage";
import SwitchPage from "./pages/SwitchPage";
import TableBasicPage from "./pages/TableBasicPage";
import TableDataPage from "./pages/TableDataPage";
import TabsPage from "./pages/TabsPage";
import TagsPage from "./pages/TagsPage";
import TermsConditionPage from "./pages/TermsConditionPage";
import TextGeneratorPage from "./pages/TextGeneratorPage";
import ThemePage from "./pages/ThemePage";
import TooltipPage from "./pages/TooltipPage";
import TypographyPage from "./pages/TypographyPage";
import UsersGridPage from "./pages/UsersGridPage";
import UsersListPage from "./pages/UsersListPage";
import ViewDetailsPage from "./pages/ViewDetailsPage";
import VideoGeneratorPage from "./pages/VideoGeneratorPage";
import VideosPage from "./pages/VideosPage";
import ViewProfilePage from "./pages/ViewProfilePage";
import VoiceGeneratorPage from "./pages/VoiceGeneratorPage";
import WalletPage from "./pages/WalletPage";
import WidgetsPage from "./pages/WidgetsPage";
import WizardPage from "./pages/WizardPage";
import RouteScrollToTop from "./helper/RouteScrollToTop";
import TextGeneratorNewPage from "./pages/TextGeneratorNewPage";
import HomePageEight from "./pages/HomePageEight";
import HomePageNine from "./pages/HomePageNine";
import HomePageTen from "./pages/HomePageTen";
import HomePageEleven from "./pages/HomePageEleven";
import GalleryGridPage from "./pages/GalleryGridPage";
import GalleryMasonryPage from "./pages/GalleryMasonryPage";
import GalleryHoverPage from "./pages/GalleryHoverPage";
import BlogPage from "./pages/BlogPage";
import BlogDetailsPage from "./pages/BlogDetailsPage";
import AddBlogPage from "./pages/AddBlogPage";
import TestimonialsPage from "./pages/TestimonialsPage";
import ComingSoonPage from "./pages/ComingSoonPage";
import AccessDeniedPage from "./pages/AccessDeniedPage";
import MaintenancePage from "./pages/MaintenancePage";
import BlankPagePage from "./pages/BlankPagePage";
import MaterialCardPage from "./pages/MaterialCardPage";
import MaterialCardTrashPage from './pages/MaterialCardTrashPage';
import MaterialCategoryPage from './pages/MaterialCategoryPage';
import MaterialCategoryTrashPage from './pages/MaterialCategoryTrashPage';
import MaterialMovementPage from './pages/MaterialMovementPage';
import MaterialMovementTrashPage from './pages/MaterialMovementTrashPage';
import CreateRolePage from './pages/CreateRolePage';

function App() {
  return (
    <AuthProvider>
    <BrowserRouter>
      <RouteScrollToTop />
      <Routes>
          {/* Authentication Routes */}
          <Route 
            path='/sign-in' 
            element={
              <ProtectedRoute requireAuth={false}>
                <SignInPage />
              </ProtectedRoute>
            } 
          />
          
          {/* Dashboard Routes - Protected */}
        <Route
            path='/' 
            element={
              <ProtectedRoute>
                <HomePageTen />
              </ProtectedRoute>
            } 
          />
          
          {/* User Management Routes - Admin Only */}
          <Route 
            path='/users-list' 
            element={
              <ProtectedRoute adminOnly={true}>
                <UsersListPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/add-user' 
            element={
              <ProtectedRoute adminOnly={true}>
                <AddUserPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/assign-role' 
            element={
              <ProtectedRoute adminOnly={true}>
                <AssignRolePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/create-role' 
            element={
              <ProtectedRoute adminOnly={true}>
                <CreateRolePage />
              </ProtectedRoute>
            } 
          />
          
          {/* Profile Routes - Authenticated Users */}
          <Route 
            path='/view-profile' 
            element={
              <ProtectedRoute>
                <ViewProfilePage />
              </ProtectedRoute>
            } 
          />
          
          {/* Material Management Routes - Protected */}
          <Route 
            path='/material-cards' 
            element={
              <ProtectedRoute>
                <MaterialCardPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-card-trash' 
            element={
              <ProtectedRoute>
                <MaterialCardTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-category' 
            element={
              <ProtectedRoute>
                <MaterialCategoryPage />
              </ProtectedRoute>
            } 
          />
        <Route
            path='/material-category-trash' 
            element={
              <ProtectedRoute>
                <MaterialCategoryTrashPage />
              </ProtectedRoute>
            } 
          />
        <Route
            path='/material-movements' 
            element={
              <ProtectedRoute>
                <MaterialMovementPage />
              </ProtectedRoute>
            } 
          />
        <Route
            path='/material-movement-trash' 
            element={
              <ProtectedRoute>
                <MaterialMovementTrashPage />
              </ProtectedRoute>
            } 
          />

          {/* Other Protected Routes */}
          <Route path='/index-2' element={<ProtectedRoute><HomePageTwo /></ProtectedRoute>} />
          <Route path='/index-3' element={<ProtectedRoute><HomePageThree /></ProtectedRoute>} />
          <Route path='/index-4' element={<ProtectedRoute><HomePageFour /></ProtectedRoute>} />
          <Route path='/index-5' element={<ProtectedRoute><HomePageFive /></ProtectedRoute>} />
          <Route path='/index-6' element={<ProtectedRoute><HomePageSix /></ProtectedRoute>} />
          <Route path='/index-7' element={<ProtectedRoute><HomePageSeven /></ProtectedRoute>} />
          <Route path='/index-8' element={<ProtectedRoute><HomePageEight /></ProtectedRoute>} />
          <Route path='/index-9' element={<ProtectedRoute><HomePageNine /></ProtectedRoute>} />
          <Route path='/index-10' element={<ProtectedRoute><HomePageTen /></ProtectedRoute>} />
          <Route path='/index-11' element={<ProtectedRoute><HomePageEleven /></ProtectedRoute>} />

          <Route path='/alert' element={<ProtectedRoute><AlertPage /></ProtectedRoute>} />
          <Route path='/avatar' element={<ProtectedRoute><AvatarPage /></ProtectedRoute>} />
          <Route path='/badges' element={<ProtectedRoute><BadgesPage /></ProtectedRoute>} />
          <Route path='/button' element={<ProtectedRoute><ButtonPage /></ProtectedRoute>} />
          <Route path='/calendar-main' element={<ProtectedRoute><CalendarMainPage /></ProtectedRoute>} />
          <Route path='/calendar' element={<ProtectedRoute><CalendarMainPage /></ProtectedRoute>} />
          <Route path='/card' element={<ProtectedRoute><CardPage /></ProtectedRoute>} />
          <Route path='/carousel' element={<ProtectedRoute><CarouselPage /></ProtectedRoute>} />
          <Route path='/chat-empty' element={<ProtectedRoute><ChatEmptyPage /></ProtectedRoute>} />
          <Route path='/chat-message' element={<ProtectedRoute><ChatMessagePage /></ProtectedRoute>} />
          <Route path='/chat-profile' element={<ProtectedRoute><ChatProfilePage /></ProtectedRoute>} />
          <Route path='/code-generator' element={<ProtectedRoute><CodeGeneratorPage /></ProtectedRoute>} />
          <Route path='/code-generator-new' element={<ProtectedRoute><CodeGeneratorNewPage /></ProtectedRoute>} />
          <Route path='/colors' element={<ProtectedRoute><ColorsPage /></ProtectedRoute>} />
          <Route path='/column-chart' element={<ProtectedRoute><ColumnChartPage /></ProtectedRoute>} />
          <Route path='/company' element={<ProtectedRoute><CompanyPage /></ProtectedRoute>} />
          <Route path='/currencies' element={<ProtectedRoute><CurrenciesPage /></ProtectedRoute>} />
          <Route path='/dropdown' element={<ProtectedRoute><DropdownPage /></ProtectedRoute>} />
          <Route path='/email' element={<ProtectedRoute><EmailPage /></ProtectedRoute>} />
          <Route path='/faq' element={<ProtectedRoute><FaqPage /></ProtectedRoute>} />
          <Route path='/forgot-password' element={<ProtectedRoute><ForgotPasswordPage /></ProtectedRoute>} />
          <Route path='/form-layout' element={<ProtectedRoute><FormLayoutPage /></ProtectedRoute>} />
          <Route path='/form-validation' element={<ProtectedRoute><FormValidationPage /></ProtectedRoute>} />
          <Route path='/form' element={<ProtectedRoute><FormPage /></ProtectedRoute>} />

          <Route path='/gallery' element={<ProtectedRoute><GalleryPage /></ProtectedRoute>} />
          <Route path='/gallery-grid' element={<ProtectedRoute><GalleryGridPage /></ProtectedRoute>} />
          <Route path='/gallery-masonry' element={<ProtectedRoute><GalleryMasonryPage /></ProtectedRoute>} />
          <Route path='/gallery-hover' element={<ProtectedRoute><GalleryHoverPage /></ProtectedRoute>} />

          <Route path='/blog' element={<ProtectedRoute><BlogPage /></ProtectedRoute>} />
          <Route path='/blog-details' element={<ProtectedRoute><BlogDetailsPage /></ProtectedRoute>} />
          <Route path='/add-blog' element={<ProtectedRoute><AddBlogPage /></ProtectedRoute>} />

          <Route path='/testimonials' element={<ProtectedRoute><TestimonialsPage /></ProtectedRoute>} />
          <Route path='/coming-soon' element={<ComingSoonPage />} />
          <Route path='/access-denied' element={<AccessDeniedPage />} />
          <Route path='/maintenance' element={<MaintenancePage />} />
          <Route path='/blank-page' element={<ProtectedRoute><BlankPagePage /></ProtectedRoute>} />

          <Route path='/image-generator' element={<ProtectedRoute><ImageGeneratorPage /></ProtectedRoute>} />
          <Route path='/image-upload' element={<ProtectedRoute><ImageUploadPage /></ProtectedRoute>} />
          <Route path='/invoice-add' element={<ProtectedRoute><InvoiceAddPage /></ProtectedRoute>} />
          <Route path='/invoice-edit' element={<ProtectedRoute><InvoiceEditPage /></ProtectedRoute>} />
          <Route path='/invoice-list' element={<ProtectedRoute><InvoiceListPage /></ProtectedRoute>} />
          <Route path='/invoice-preview' element={<ProtectedRoute><InvoicePreviewPage /></ProtectedRoute>} />
          <Route path='/kanban' element={<ProtectedRoute><KanbanPage /></ProtectedRoute>} />
          <Route path='/language' element={<ProtectedRoute><LanguagePage /></ProtectedRoute>} />
          <Route path='/line-chart' element={<ProtectedRoute><LineChartPage /></ProtectedRoute>} />
          <Route path='/list' element={<ProtectedRoute><ListPage /></ProtectedRoute>} />
          <Route path='/marketplace-details' element={<ProtectedRoute><MarketplaceDetailsPage /></ProtectedRoute>} />
          <Route path='/marketplace' element={<ProtectedRoute><MarketplacePage /></ProtectedRoute>} />
          <Route path='/notification-alert' element={<ProtectedRoute><NotificationAlertPage /></ProtectedRoute>} />
          <Route path='/notification' element={<ProtectedRoute><NotificationPage /></ProtectedRoute>} />
          <Route path='/pagination' element={<ProtectedRoute><PaginationPage /></ProtectedRoute>} />
          <Route path='/payment-gateway' element={<ProtectedRoute><PaymentGatewayPage /></ProtectedRoute>} />
          <Route path='/pie-chart' element={<ProtectedRoute><PieChartPage /></ProtectedRoute>} />
          <Route path='/portfolio' element={<ProtectedRoute><PortfolioPage /></ProtectedRoute>} />
          <Route path='/pricing' element={<ProtectedRoute><PricingPage /></ProtectedRoute>} />
          <Route path='/progress' element={<ProtectedRoute><ProgressPage /></ProtectedRoute>} />
          <Route path='/radio' element={<ProtectedRoute><RadioPage /></ProtectedRoute>} />
          <Route path='/role-access' element={<ProtectedRoute><RoleAccessPage /></ProtectedRoute>} />
          <Route path='/sign-up' element={<ProtectedRoute><SignUpPage /></ProtectedRoute>} />
          <Route path='/star-rating' element={<ProtectedRoute><StarRatingPage /></ProtectedRoute>} />
          <Route path='/starred' element={<ProtectedRoute><StarredPage /></ProtectedRoute>} />
          <Route path='/switch' element={<ProtectedRoute><SwitchPage /></ProtectedRoute>} />
          <Route path='/table-basic' element={<ProtectedRoute><TableBasicPage /></ProtectedRoute>} />
          <Route path='/table-data' element={<ProtectedRoute><TableDataPage /></ProtectedRoute>} />
          <Route path='/tabs' element={<ProtectedRoute><TabsPage /></ProtectedRoute>} />
          <Route path='/tags' element={<ProtectedRoute><TagsPage /></ProtectedRoute>} />
          <Route path='/terms-condition' element={<ProtectedRoute><TermsConditionPage /></ProtectedRoute>} />
          <Route path='/text-generator-new' element={<ProtectedRoute><TextGeneratorNewPage /></ProtectedRoute>} />
          <Route path='/text-generator' element={<ProtectedRoute><TextGeneratorPage /></ProtectedRoute>} />
          <Route path='/theme' element={<ProtectedRoute><ThemePage /></ProtectedRoute>} />
          <Route path='/tooltip' element={<ProtectedRoute><TooltipPage /></ProtectedRoute>} />
          <Route path='/typography' element={<ProtectedRoute><TypographyPage /></ProtectedRoute>} />
          <Route path='/users-grid' element={<ProtectedRoute><UsersGridPage /></ProtectedRoute>} />
          <Route path='/view-details' element={<ProtectedRoute><ViewDetailsPage /></ProtectedRoute>} />
          <Route path='/video-generator' element={<ProtectedRoute><VideoGeneratorPage /></ProtectedRoute>} />
          <Route path='/videos' element={<ProtectedRoute><VideosPage /></ProtectedRoute>} />
          <Route path='/voice-generator' element={<ProtectedRoute><VoiceGeneratorPage /></ProtectedRoute>} />
          <Route path='/wallet' element={<ProtectedRoute><WalletPage /></ProtectedRoute>} />
          <Route path='/widgets' element={<ProtectedRoute><WidgetsPage /></ProtectedRoute>} />
          <Route path='/wizard' element={<ProtectedRoute><WizardPage /></ProtectedRoute>} />

          <Route path='*' element={<ErrorPage />} />
      </Routes>
    </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
