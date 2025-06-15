<?php

namespace src\Controller\Admin;

use src\Controller\Admin\Field\VichImageField;
use src\Entity\Event;
use App\Entity\EventImages;
use EasyCorp\Bundle\EasyAdminBundle\Config\Assets;
use EasyCorp\Bundle\EasyAdminBundle\Config\Crud;
use EasyCorp\Bundle\EasyAdminBundle\Controller\AbstractCrudController;
use EasyCorp\Bundle\EasyAdminBundle\Field\AssociationField;
use EasyCorp\Bundle\EasyAdminBundle\Field\ChoiceField;
use EasyCorp\Bundle\EasyAdminBundle\Field\CollectionField;
use EasyCorp\Bundle\EasyAdminBundle\Field\DateTimeField;
use EasyCorp\Bundle\EasyAdminBundle\Field\FormField;
use EasyCorp\Bundle\EasyAdminBundle\Field\HiddenField;
use EasyCorp\Bundle\EasyAdminBundle\Field\IdField;
use EasyCorp\Bundle\EasyAdminBundle\Field\IntegerField;
use EasyCorp\Bundle\EasyAdminBundle\Field\NumberField;
use EasyCorp\Bundle\EasyAdminBundle\Field\TextEditorField;
use EasyCorp\Bundle\EasyAdminBundle\Field\TextField;
use EasyCorp\Bundle\EasyAdminBundle\Field\TimeField;

class EventCrudController extends AbstractCrudController
{
    public static function getEntityFqcn(): string
    {
        return Event::class;
    }

    public function configureCrud(Crud $crud): Crud
    {
        return parent::configureCrud($crud)
            ->setFormThemes(['admin/field/schema.html.twig', '@EasyAdmin/crud/form_theme.html.twig'])
            ->setEntityLabelInPlural('Мероприятия')
            ->setEntityLabelInSingular('мероприятие')
            ->setPageTitle(Crud::PAGE_NEW, 'Добавление мероприятия')
            ->setPageTitle(Crud::PAGE_EDIT, 'Изменение мероприятия');
    }

    public function configureAssets(Assets $assets): Assets
    {
        $assets->addWebpackEncoreEntry('schema');
        return parent::configureAssets($assets);
    }

    public function configureFields(string $pageName): iterable
    {
        yield IdField::new('id', 'ID')->hideOnForm();

        yield FormField::addTab('Основная информация');
        yield TextField::new('title', 'Название')
            ->setColumns(8);
        yield TextEditorField::new('description', 'Описание')
            ->setColumns(8);
        yield TextEditorField::new('shortDescription', 'Краткое описание')
            ->setColumns(8);

        yield DateTimeField::new('dateTimeAt', 'Дата и время')
            ->setColumns(8);

        yield ChoiceField::new('type', 'Тип')
            ->setChoices(
                [
                    'Неограниченное количество мест' => 'Неограниченное количество мест',
                    'Ограниченное количество мест' => 'Ограниченное количество мест',
                    'Места согласно билетам' => 'Места согласно билетам',
                ]
            )
            ->setColumns(8);
        yield ChoiceField::new('age', 'Возраст')
            ->setChoices
            (
                [
                    '0+' => '0+',
                    '6+' => '6+',
                    '12+' => '12+',
                    '16+' => '16+',
                    '18+' => '18+',
                ]
            )
            ->setColumns(8);

        yield TimeField::new('duration', 'Длительность')
            ->setColumns(8);

        yield IntegerField::new('places', 'Места (количество)')
            ->setColumns(8);

        yield AssociationField::new('news', 'Новости')
            ->hideOnForm();

        yield NumberField::new('price', 'Цена')
            ->setColumns(8);

        yield VichImageField::new('imageFile', 'Изображение')
            ->setRequired(false)
            ->setFormTypeOption('allow_delete', false)
            ->onlyOnForms()
            ->setColumns(8);

        yield FormField::addTab('Схема зала');
        yield AssociationField::new('scheme', 'Схема зала')
            ->setColumns(8);
        yield HiddenField::new('schemeWidget')
            ->setFormTypeOptions([
                'block_name' => 'schema',
            ])
            ->onlyOnForms();
        yield HiddenField::new('schemeData')
            ->onlyOnForms();
    }
}
